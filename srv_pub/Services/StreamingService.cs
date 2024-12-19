using Grpc.Core;
using GrpcClient;
using Server.Services.BackgroundJobs;
using Server.Model;
using AutoMapper;
using outbox;
using Azure;
using System.Threading;
using System.Security;

namespace Server.Services
{
    public class StreamingService : StreamingDates.StreamingDatesBase
    {
        protected readonly CancellationTokenSource _tocken = new CancellationTokenSource();
        private IConfiguration _configuration;
        private readonly ILogger<StreamingService> _logger;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        private readonly IOutbox _outbox;
        private readonly IMapper _mapper;
        private readonly string _topic;
        public StreamingService(IConfiguration configuration,
                                ILogger<StreamingService> logger, 
                                IMapper mapper,
                                IBackgroundJobs<Snapshot> backgroundJobs,
                                IOutbox outbox )
        {
            _configuration = configuration;
            _backgroundJobs = backgroundJobs;
            _logger = logger;
            _mapper = mapper;
            _outbox = outbox;
            _topic = _configuration["Topic"];
        }

        public override async Task<streamingReply> GetStreamingData(IAsyncStreamReader<streamingRequest> requestStream, ServerCallContext context)
        {
            Snapshot _data; streamingRequest _message;
            var httpContext = context.GetHttpContext();
            _logger.LogInformation($"Connection id: {httpContext.Connection.Id}");

            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                _message = requestStream.Current;
                _data = _mapper.Map<Snapshot>(_message);
                // _backgroundJobs.BackgroundTaskSignalR.Enqueue(_data);
                await _outbox.AddAsync<Snapshot>(
                data: _data,
                topic: _topic,
                partitionBy: i=> i.dest_ip,
                isSequential: false,
                metadata: new Dictionary<string, string>
                {
                    { "OperationId",_data.dest_ip }
                },
                cancellationToken: _tocken.Token);

                _logger.LogInformation($"{_message.SourceIp}:{_message.SourceIp},{_message.DestIp} :{_message.DestPort},{_message.SourceMac},{_message.DestMac}  ");
            }

            return new streamingReply { Index = 1 };
        }

    }
}
