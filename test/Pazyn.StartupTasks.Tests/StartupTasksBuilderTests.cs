using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Pazyn.StartupTasks.Tests
{
    public class StartupTasksBuilderTests
    {
        [Fact]
        public void Decorate_DecoratesLastItem()
        {
            var services = new ServiceCollection();
            services.AddStartupTasks()
                .AddStartupTask<EmptyStartupTask>()
                .Decorate<EmptyStartupTaskDecorator1>()
                .Decorate<EmptyStartupTaskDecorator2>()
                .AddStartupTask<AnotherEmptyStartupTask>();

            var serviceProvider = services.BuildServiceProvider();
            var startupTaskItemsCollection = serviceProvider.GetRequiredService<IStartupTaskItemsCollection>();
            var startupTasks = startupTaskItemsCollection.Select(x => x.TaskFactory(serviceProvider)).ToArray();

            Assert.Collection(startupTasks,
                startupTask => Assert.IsType<EmptyStartupTaskDecorator2>(startupTask),
                startupTask => Assert.IsType<AnotherEmptyStartupTask>(startupTask));
        }
    }
}