using Grpc.Core;
using GrpcClient;

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

            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var message = requestStream.Current;
                _logger.LogInformation($"{message.SourceIp}:{message.SourceIp},{ message.DestIp} :{message.DestPort},{ message.SourceMac},{ message.DestMac}  ");
            }

            return new streamingReply { Index = 1 };
        }

    }
}