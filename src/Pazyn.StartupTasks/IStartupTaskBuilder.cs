using System;

namespace Pazyn.StartupTasks
{
    public interface IStartupTaskBuilder
    {
        IStartupTaskBuilder AddStartupTask<T>(String displayName = default) where T : IStartupTask;
        IStartupTaskBuilder AddStartupTask(Func<IStartupTask> taskFactory, String displayName);
        IStartupTaskBuilder AddStartupTask(Func<IServiceProvider, IStartupTask> taskFactory, String displayName);

        IStartupTaskBuilder Decorate<TDec>() where TDec : IStartupTask;
        IStartupTaskBuilder Decorate(Func<IStartupTask, IStartupTask> taskFactory);
        IStartupTaskBuilder Decorate(Func<IServiceProvider, IStartupTask, IStartupTask> taskFactory);
    }
}