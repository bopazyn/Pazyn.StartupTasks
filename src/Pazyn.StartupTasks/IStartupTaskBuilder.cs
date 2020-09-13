using System;

namespace Pazyn.StartupTasks
{
    public interface IStartupTaskBuilder
    {
        IStartupTaskBuilder AddStartupTask<T>(Action<StartupTaskItem> configureItem = null) where T : IStartupTask;
        IStartupTaskBuilder AddStartupTask(Func<IStartupTask> taskFactory, Action<StartupTaskItem> configureItem = null);
        IStartupTaskBuilder AddStartupTask(Func<IServiceProvider, IStartupTask> taskFactory, Action<StartupTaskItem> configureItem = null);
    }
}