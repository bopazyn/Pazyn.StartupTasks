using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pazyn.StartupTasks
{
    internal class StartupTaskHostedService : BackgroundService
    {
        private IServiceProvider ServiceProvider { get; }
        private ILogger<StartupTaskHostedService> Logger { get; }
        private IStartupTaskItemsCollection StartupTaskItems { get; }
        private StartupTaskContext StartupTaskContext { get; }

        public StartupTaskHostedService(IServiceProvider serviceProvider, ILogger<StartupTaskHostedService> logger, StartupTaskContext startupTaskContext, IStartupTaskItemsCollection startupTaskItems)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
            StartupTaskContext = startupTaskContext;
            StartupTaskItems = startupTaskItems;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var startupTaskItem in StartupTaskItems)
            {
                try
                {
                    using var scope = ServiceProvider.CreateScope();
                    var startupTask = startupTaskItem.TaskFactory(scope.ServiceProvider);
                    var startupTaskResult = await startupTask.Run(stoppingToken);
                    if (startupTaskResult)
                    {
                        StartupTaskContext.MarkTaskAsComplete();
                        Logger.LogInformation("Startup task {0} completed.", startupTaskItem.DisplayName);
                    }
                    else
                    {
                        StartupTaskContext.MarkTaskAsFailed();
                        Logger.LogError("Startup task {0} failed.", startupTaskItem.DisplayName);
                    }
                }
                catch (Exception ex)
                {
                    StartupTaskContext.MarkTaskAsFailed();
                    Logger.LogCritical(ex, "An exception was thrown during execution of Startup task {0}.", startupTaskItem.DisplayName);
                }
            }
        }
    }
}