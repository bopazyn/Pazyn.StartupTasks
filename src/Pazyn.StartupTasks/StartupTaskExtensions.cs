using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pazyn.StartupTasks
{
    public static class StartupTaskExtensions
    {
        public static IStartupTaskBuilder AddStartupTasks(this IServiceCollection services)
        {
            var startupTaskBuilder = new StartupTaskBuilder(services);
            services.TryAddSingleton<IStartupTaskBuilder>(startupTaskBuilder);
            services.AddHostedService<StartupTaskHostedService>();

            services.Configure<StartupTaskContext>(options => { });
            return startupTaskBuilder;
        }

        public static IHealthChecksBuilder AddStartupTasks(this IHealthChecksBuilder builder) =>
            builder.AddCheck<StartupTasksHealthCheck>("Startup tasks");

        public static IApplicationBuilder UseStartupTasks(this IApplicationBuilder builder) =>
            builder.UseMiddleware<StartupTasksMiddleware>();

        public static IEndpointConventionBuilder RequireStartupTask(this IEndpointConventionBuilder endpoint)
        {
            endpoint.WithMetadata(new StartupTaskMetadata());
            return endpoint;
        }
    }
}