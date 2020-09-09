using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pazyn.StartupTasks
{
    internal class StartupTasksHealthCheck : IHealthCheck
    {
        private StartupTaskContext Context { get; }

        public StartupTasksHealthCheck(StartupTaskContext context)
        {
            Context = context;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (!Context.HaveAllTasksFinished)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Startup tasks have not finished"));
            }

            if (Context.HasAnyTaskFailed)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Startup task failed"));
            }

            return Task.FromResult(HealthCheckResult.Healthy("All startup tasks have completed"));
        }
    }
}