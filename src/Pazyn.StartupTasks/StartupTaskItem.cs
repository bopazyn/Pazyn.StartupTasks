using System;
using Microsoft.Extensions.DependencyInjection;

namespace Pazyn.StartupTasks
{
    public class StartupTaskItem
    {
        public Func<IServiceProvider, IStartupTask> TaskFactory { get; private set; }

        public StartupTaskItem(Func<IServiceProvider, IStartupTask> taskFactory)
        {
            TaskFactory = taskFactory;
        }

        public String DisplayName { get; set; }

        public void Decorate<TDec>() where TDec : IStartupTask =>
            Decorate((sp, job) => ActivatorUtilities.CreateInstance<TDec>(sp, job));

        public void Decorate(Func<IStartupTask, IStartupTask> taskFactory) =>
            Decorate((sp, job) => taskFactory(job));

        public void Decorate(Func<IServiceProvider, IStartupTask, IStartupTask> taskFactory)
        {
            var tmp = TaskFactory;
            TaskFactory = sp => taskFactory(sp, tmp(sp));
        }
    }
}