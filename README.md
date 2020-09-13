# Pazyn.StartupTasks

`Pazyn.StartupTasks` is library for running tasks when application is starting. For example, you can migrate your database, validate configuration, synchronize permission or populate cache.
Startup tasks can be configured separately to work in two modes: blocking and nonblocking. Blocking means that all request are blocked until the tasks have completed. Second mode guards routes that are marked with medod `RequireStartupTask` producing 503 HTTP response code until tasks have completed.


## Minimal working example

```
public class SillyStartupTask : IStartupTask
{
    public Task<Boolean> Run(CancellationToken cancellationToken) => Task.FromResult(true);
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddStartupTasks();

        services.AddStartupTasks()
            .AddStartupTask<SillyStartupTask>();
    }

    public void Configure(IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseRouting();
        app.UseStartupTasks();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health");

            endpoints.MapGet("/", context => context.Response.WriteAsync("Hello World!"))
                .RequireStartupTask();
        });
    }
}
```


## Acknowledgements

The work was inspired by:
- [Andrew Lock](https://github.com/andrewlock) and his [blog](https://andrewlock.net/)

Look at his blog posts:
1. https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/
1. https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-2/
1. https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-3-feedback/
1. https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-4-using-health-checks/
1. https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/
