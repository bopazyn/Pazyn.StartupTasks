# Pazyn.StartupTasks

The work was inpired by:
- [Andrew Lock](https://github.com/andrewlock) and his [blog](https://andrewlock.net/)

## Example

Detail explanation will be available soon.
```
public class AlwaysTrueStartupTask : IStartupTask
{
    public Task<Boolean> Run(CancellationToken cancellationToken) => Task.FromResult(true);
}

public class AlwaysFalseStartupTask : IStartupTask
{
    public Task<Boolean> Run(CancellationToken cancellationToken) => Task.FromResult(false);
}

public class DelayStartupTaskDecorator : IStartupTask
{
    private IStartupTask InnerStartupTask { get; }

    public DelayStartupTaskDecorator(IStartupTask innerStartupTask)
    {
        InnerStartupTask = innerStartupTask;
    }

    public async Task<Boolean> Run(CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);
        return await InnerStartupTask.Run(cancellationToken);
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddStartupTasks();

        services.AddStartupTasks()
            .AddStartupTask<AlwaysTrueStartupTask>()
            .AddStartupTask<AlwaysFalseStartupTask>()
            .Decorate<DelayStartupTaskDecorator>();
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