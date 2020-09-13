using System;
using Microsoft.Extensions.DependencyInjection;

namespace Pazyn.StartupTasks
{
    internal class StartupTaskBuilder : IStartupTaskBuilder
    {
        private IServiceCollection Services { get; }

        public StartupTaskBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IStartupTaskBuilder AddStartupTask<T>(Action<StartupTaskItem> configureItem = null) where T : IStartupTask =>
            AddStartupTask(sp => ActivatorUtilities.CreateInstance<T>(sp), configureItem);

        public IStartupTaskBuilder AddStartupTask(Func<IStartupTask> taskFactory, Action<StartupTaskItem> configureItem = null) =>
            AddStartupTask(sp => taskFactory(), configureItem);

        public IStartupTaskBuilder AddStartupTask(Func<IServiceProvider, IStartupTask> taskFactory, Action<StartupTaskItem> configureItem = null)
        {
            var startupTaskItem = new StartupTaskItem(taskFactory);
            configureItem?.Invoke(startupTaskItem);

            Services.Configure<StartupTaskContext>(options =>
            {
                options.RegisterTask();
                options.Items.Add(startupTaskItem);
                startupTaskItem.DisplayName = $"StartupTask #{options.Items.Count + 1}";
            });

            return this;
        }
    }
}