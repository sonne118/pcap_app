using Grpc.Core;
using GrpcClient;
using Server.Services.BackgroundJobs;
using Server.Model;
using AutoMapper;

namespace Server.Services
{
    public class StreamingService : StreamingDates.StreamingDatesBase
    {
        private readonly ILogger<StreamingService> _logger;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        private readonly IMapper _mapper;
        public StreamingService(ILogger<StreamingService> logger, IMapper mapper, IBackgroundJobs<Snapshot> backgroundJobs)
        {
            _backgroundJobs = backgroundJobs;
            _logger = logger;
            _mapper = mapper;
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
                _logger.LogInformation($"{_message.SourceIp}:{_message.SourceIp},{_message.DestIp} :{_message.DestPort},{_message.SourceMac},{_message.DestMac}  ");
            }

            return new streamingReply { Index = 1 };
        }

    }
}
