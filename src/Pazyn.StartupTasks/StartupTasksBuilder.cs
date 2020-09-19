using System;
using Microsoft.Extensions.DependencyInjection;

namespace Pazyn.StartupTasks
{
    internal class StartupTasksBuilder : IStartupTasksBuilder
    {
        private IServiceCollection Services { get; }

        public StartupTasksBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IStartupTasksBuilder AddStartupTask<T>(Action<StartupTaskItem> configureItem = null) where T : IStartupTask =>
            AddStartupTask(sp => ActivatorUtilities.CreateInstance<T>(sp), configureItem);

        public IStartupTasksBuilder AddStartupTask(Func<IStartupTask> taskFactory, Action<StartupTaskItem> configureItem = null) =>
            AddStartupTask(sp => taskFactory(), configureItem);

        public IStartupTasksBuilder AddStartupTask(Func<IServiceProvider, IStartupTask> taskFactory, Action<StartupTaskItem> configureItem = null)
        {
            var startupTaskItem = new StartupTaskItem(taskFactory);
            configureItem?.Invoke(startupTaskItem);

            Services.Configure<StartupTasksContext>(options =>
            {
                options.RegisterTask();
                options.Items.Add(startupTaskItem);
                startupTaskItem.DisplayName = $"StartupTask #{options.Items.Count + 1}";
            });

            return this;
        }
    }
}