using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Pazyn.StartupTasks
{
    internal class StartupTasksHealthCheck : IHealthCheck
    {
        private IOptions<StartupTasksContext> Context { get; }

        public StartupTasksHealthCheck(IOptions<StartupTasksContext> context)
        {
            Context = context;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (!Context.Value.HaveAllTasksFinished)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Startup tasks have not finished"));
            }

            if (Context.Value.HasAnyTaskFailed)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Startup task failed"));
            }

            return Task.FromResult(HealthCheckResult.Healthy("All startup tasks have completed"));
        }
    }
}