using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Pazyn.StartupTasks.Tests
{
    public class StartupTasksBuilderTests
    {
        [Fact]
        public void Registering_Items_With_Decorators()
        {
            var services = new ServiceCollection();
            services.AddStartupTasks()
                .AddStartupTask<EmptyStartupTask1>()
                .AddStartupTask<EmptyStartupTask2>(sti =>
                {
                    sti.Decorate<EmptyStartupTaskDecorator1>();
                    sti.Decorate<EmptyStartupTaskDecorator2>();
                });

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<StartupTasksContext>>();
            var startupTasks = options.Value.Items.Select(x => x.TaskFactory(serviceProvider)).ToArray();

            Assert.Collection(startupTasks,
                startupTask => Assert.IsType<EmptyStartupTask1>(startupTask),
                startupTask => Assert.IsType<EmptyStartupTaskDecorator2>(startupTask));
        }
    }
}