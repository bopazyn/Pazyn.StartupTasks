using System;

namespace Pazyn.StartupTasks
{
    internal class StartupTaskItem
    {
        public Func<IServiceProvider, IStartupTask> TaskFactory { get; }
        public String DisplayName { get; }

        public StartupTaskItem(Func<IServiceProvider, IStartupTask> taskFactory, String displayName)
        {
            TaskFactory = taskFactory;
            DisplayName = displayName;
        }
    }
}