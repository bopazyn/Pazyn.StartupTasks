using System;

namespace Pazyn.StartupTasks
{
    public interface IStartupTasksBuilder
    {
        IStartupTasksBuilder AddStartupTask<T>(Action<StartupTaskItem> configureItem = null) where T : IStartupTask;
        IStartupTasksBuilder AddStartupTask(Func<IStartupTask> taskFactory, Action<StartupTaskItem> configureItem = null);
        IStartupTasksBuilder AddStartupTask(Func<IServiceProvider, IStartupTask> taskFactory, Action<StartupTaskItem> configureItem = null);
    }
}