using Grpc.Core;
using GrpcClient;
using Server;
using System.Diagnostics.Metrics;

namespace Server.Services
{
    public class StreamingService : StreamingData.StreamingDataBase
    {
        private readonly ILogger<StreamingService> _logger;
        public StreamingService(ILogger<StreamingService> logger)
        {
            _logger = logger;
        }


        public override async Task<streamingReply> GetStreamingData(IAsyncStreamReader<streamingRequest> requestStream, ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            _logger.LogInformation($"Connection id: {httpContext.Connection.Id}");
            //await foreach (var message in requestStream.ReadAllAsync())
            //{
            //    _logger.LogInformation($"{message.DestIp} :{message.DestPort},   {message.SourceIp}:{message.SourceIp} ");
            //}
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var message = requestStream.Current;
                _logger.LogInformation($"{message.DestIp} :{message.DestPort},   {message.SourceIp}:{message.SourceIp} ");
            }

            return new streamingReply { Index=1 };
        }
        
    }
}
