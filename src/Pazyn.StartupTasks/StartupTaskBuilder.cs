using System;
using Microsoft.Extensions.DependencyInjection;

namespace Pazyn.StartupTasks
{
    internal class StartupTaskBuilder : IStartupTaskBuilder
    {
        private StartupTaskItem LastItem { get; }
        private IStartupTaskItemsCollection StartupTaskItems { get; }
        private StartupTaskContext StartupTaskContext { get; }

        public StartupTaskBuilder(IStartupTaskItemsCollection startupTaskItems, StartupTaskContext startupTaskContext) : this(startupTaskItems, startupTaskContext, null)
        {
        }

        private StartupTaskBuilder(IStartupTaskItemsCollection startupTaskItems, StartupTaskContext startupTaskContext, StartupTaskItem lastItem)
        {
            StartupTaskItems = startupTaskItems;
            StartupTaskContext = startupTaskContext;
            LastItem = lastItem;
        }

        public IStartupTaskBuilder AddStartupTask<T>(String displayName = default) where T : IStartupTask =>
            AddStartupTask(sp => ActivatorUtilities.CreateInstance<T>(sp), displayName ?? typeof(T).Name);

        public IStartupTaskBuilder AddStartupTask(Func<IStartupTask> taskFactory, String displayName) =>
            AddStartupTask(sp => taskFactory(), displayName);

        public IStartupTaskBuilder AddStartupTask(Func<IServiceProvider, IStartupTask> taskFactory, String displayName)
        {
            var startupTaskItem = new StartupTaskItem(taskFactory, displayName);
            StartupTaskContext.RegisterTask();
            StartupTaskItems.Add(startupTaskItem);
            return new StartupTaskBuilder(StartupTaskItems, StartupTaskContext, startupTaskItem);
        }

        public IStartupTaskBuilder Decorate<TDec>() where TDec : IStartupTask =>
            Decorate((sp, job) => ActivatorUtilities.CreateInstance<TDec>(sp, job));

        public IStartupTaskBuilder Decorate(Func<IStartupTask, IStartupTask> taskFactory) =>
            Decorate((sp, job) => taskFactory(job));

        public IStartupTaskBuilder Decorate(Func<IServiceProvider, IStartupTask, IStartupTask> taskFactory)
        {
            if (LastItem == null)
            {
                return this;
            }

            var startupTaskItem = new StartupTaskItem(
                sp => taskFactory(sp, LastItem.TaskFactory(sp)),
                LastItem.DisplayName
            );
            StartupTaskItems.Remove(LastItem);
            StartupTaskItems.Add(startupTaskItem);

            return new StartupTaskBuilder(StartupTaskItems, StartupTaskContext, startupTaskItem);
        }
    }
}