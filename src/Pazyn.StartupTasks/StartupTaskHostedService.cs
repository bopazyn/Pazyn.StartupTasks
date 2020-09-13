using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pazyn.StartupTasks
{
    internal class StartupTaskHostedService : IHostedService
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

        private CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        private async Task RunStartupItems(IEnumerable<StartupTaskItem> startupTaskItems, CancellationToken stoppingToken)
        {
            foreach (var startupTaskItem in startupTaskItems)
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RunStartupItems(StartupTaskContextOptions.Value.Items.Where(x => x.IsBlocking), cancellationToken);
            // ReSharper disable once UnusedVariable
            var notAwaitedTask = RunStartupItems(StartupTaskContextOptions.Value.Items.Where(x => !x.IsBlocking), CancellationTokenSource.Token);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}