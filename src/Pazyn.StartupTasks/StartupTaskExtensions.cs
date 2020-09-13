using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Pazyn.StartupTasks
{
    public static class StartupTaskExtensions
    {
        public static IStartupTaskBuilder AddStartupTasks(this IServiceCollection services)
        {
            services.AddHostedService<StartupTaskHostedService>();
            services.Configure<StartupTaskContext>(options => { });
            return new StartupTaskBuilder(services);
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