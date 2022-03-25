using Grpc.Core;
using Google.Protobuf;

namespace GrpcShutdownTest.Services
{
    public class ShutdownTestService : ShutdownTest.ShutdownTestBase
    {
        private readonly ILogger<ShutdownTestService> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public ShutdownTestService(ILogger<ShutdownTestService> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public override async Task<LongRunningTaskReply> LongRunningTask(LongRunningTaskRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received LongRunningTask request");
            await Task.Delay(TimeSpan.FromSeconds(10));

            var randomBytes = Enumerable.Range(1, 2_000_000).Select(i => (byte)i).ToArray();

            var response = new LongRunningTaskReply {FileContent = ByteString.CopyFrom(randomBytes)};

            _logger.LogInformation("Finished LongRunningTask request");
            return response;
        }

        public override Task<ShutdownReply> Shutdown(ShutdownRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received Shutdown request");
            _hostApplicationLifetime.StopApplication();
            
            return Task.FromResult(new ShutdownReply());
        }
    }
}