using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Pazyn.StartupTasks
{
    public static class StartupTasksExtensions
    {
        public static IStartupTasksBuilder AddStartupTasks(this IServiceCollection services)
        {
            services.AddHostedService<StartupTasksHostedService>();
            services.Configure<StartupTasksContext>(options => { });
            return new StartupTasksBuilder(services);
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