using Investcloud_Server_Test.Interfaces;
using Investcloud_Server_Test.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Investcloud_Server_Test;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var services = CreateServices();

        var processService = services.GetRequiredService<IProcessService>();
        await processService.ProcessMatrixMultiplication();
    }

    private static ServiceProvider CreateServices()
    {
        var serviceProvider = new ServiceCollection()
            .AddAppServices()
            .AddLogging(options =>
            {
                options.ClearProviders();
                options.AddConsole();
            })
            .AddHttpClient()
            .RemoveAll<IHttpMessageHandlerBuilderFilter>();  // remove http client debug logs   
        
        return serviceProvider.BuildServiceProvider();
    }
    
    private static ServiceCollection AddAppServices(this ServiceCollection services)
    {
        services
            .AddScoped<IApiOperationalService, ApiOperationalService>()
            .AddScoped<IHashingService, HashingService>()
            .AddScoped<IMatrixMultiplicationService, MatrixMultiplicationService>()
            .AddScoped<IProcessService, ProcessService>();

        return services;
    }
}
