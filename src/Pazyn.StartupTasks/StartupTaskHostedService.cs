using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pazyn.StartupTasks
{
    internal class StartupTaskHostedService : BackgroundService
    {
        private IServiceProvider ServiceProvider { get; }
        private ILogger<StartupTaskHostedService> Logger { get; }
        private IOptions<StartupTaskContext> StartupTaskContextOptions { get; }

        public StartupTaskHostedService(IServiceProvider serviceProvider, ILogger<StartupTaskHostedService> logger, IOptions<StartupTaskContext> startupTaskContextOptions)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
            StartupTaskContextOptions = startupTaskContextOptions;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var startupTaskItem in StartupTaskContextOptions.Value.Items)
            {
                try
                {
                    using var scope = ServiceProvider.CreateScope();
                    var startupTask = startupTaskItem.TaskFactory(scope.ServiceProvider);
                    var startupTaskResult = await startupTask.Run(stoppingToken);
                    if (startupTaskResult)
                    {
                        StartupTaskContextOptions.Value.MarkTaskAsComplete();
                        Logger.LogInformation("Startup task {0} completed.", startupTaskItem.DisplayName);
                    }
                    else
                    {
                        StartupTaskContextOptions.Value.MarkTaskAsFailed();
                        Logger.LogError("Startup task {0} failed.", startupTaskItem.DisplayName);
                    }
                }
                catch (Exception ex)
                {
                    StartupTaskContextOptions.Value.MarkTaskAsFailed();
                    Logger.LogCritical(ex, "An exception was thrown during execution of Startup task {0}.", startupTaskItem.DisplayName);
                }
            }
        }
    }
}