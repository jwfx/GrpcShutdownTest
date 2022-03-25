// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;

Console.WriteLine("Hello, World!");

var serviceConfig = new ServiceConfig
{
    MethodConfigs =
            {
                new MethodConfig
                {
                    Names = { MethodName.Default },
                    RetryPolicy = new RetryPolicy
                    {
                        MaxAttempts = 5,
                        InitialBackoff = TimeSpan.FromSeconds(8),
                        MaxBackoff = TimeSpan.FromMinutes(2),
                        BackoffMultiplier = 1.5,
                        RetryableStatusCodes = { StatusCode.Unavailable }
                    }
                }
            }
};


var channel = GrpcChannel.ForAddress("https://localhost:9000", new GrpcChannelOptions
{
    /* ServiceConfig = serviceConfig */
});

var client = new GrpcShutdownTest.ShutdownTest.ShutdownTestClient(channel);

client.LongRunningTask(new GrpcShutdownTest.LongRunningTaskRequest());

Console.WriteLine("Client finished");
