using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pazyn.StartupTasks.Example
{
    public class GreetUserStartupTask : IStartupTask
    {
        private String Name { get; }

        public GreetUserStartupTask(String name)
        {
            Name = name;
        }

        public async Task<Boolean> Run(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Hello {Name}");
            await Task.Delay(1000, cancellationToken);
            return true;
        }
    }

    public class RepeatStartupTaskDecorator : IStartupTask
    {
        private IStartupTask InnerStartupTask { get; }
        private Int32 Count { get; }

        public RepeatStartupTaskDecorator(IStartupTask innerStartupTask, Int32 count)
        {
            InnerStartupTask = innerStartupTask;
            Count = count;
        }

        public async Task<Boolean> Run(CancellationToken cancellationToken)
        {
            for (var i = 0; i < Count; i++)
            {
                await InnerStartupTask.Run(cancellationToken);
            }

            return true;
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddStartupTasks();

            services.AddStartupTasks()
                .AddStartupTask(sp => new GreetUserStartupTask("Alice"), sti =>
                {
                    sti.IsBlocking = true;
                    sti.Decorate((sp, st) => new RepeatStartupTaskDecorator(st, 2));
                })
                .AddStartupTask(sp => new GreetUserStartupTask("Bob"), sti =>
                {
                    sti.IsBlocking = false;
                    sti.Decorate((sp, st) => new RepeatStartupTaskDecorator(st, 10));
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseStartupTasks();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapGet("/", context => context.Response.WriteAsync("The route isn't blocked"));
                endpoints.MapGet("/blocking", context => context.Response.WriteAsync("The route is blocked")).RequireStartupTask();
            });
        }
    }
}