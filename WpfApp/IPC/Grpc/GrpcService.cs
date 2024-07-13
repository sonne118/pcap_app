using AutoMapper;
using CoreModel.Model;
using Grpc.Core;
using GrpcClient;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WpfApp.Model;
using WpfApp.Services.BackgroundJob;

namespace wpfapp.IPC.Grpc
{
    public class GrpcService : BackgroundService
    {
        private readonly IMapper _mapper;
        private readonly IBackgroundJobs<Snapshot> _backgroundJobs;
        private StreamingData.StreamingDataClient _streamDataClient;
        private AsyncDuplexStreamingCall<streamingRequest, streamingReply>? _clientStreamingCall;

        private CancellationTokenSource cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        public GrpcService(IBackgroundJobs<Snapshot> backgroundJobs, IMapper mapper, StreamingData.StreamingDataClient streamDataClient)
        {
            _backgroundJobs = backgroundJobs;
            _streamDataClient = streamDataClient;
            _mapper = mapper;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {            
            _clientStreamingCall = _streamDataClient.GetStreamingData(cancellationToken: cancellationToken);

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                while (_backgroundJobs.BackgroundTasks.TryPeek(out var data))
                {
                    var streamData = _mapper.Map<streamingRequest>(data);

                    await _clientStreamingCall.RequestStream.WriteAsync(streamData);

                    await Task.Delay(100, cancellationToken);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

    }
}
