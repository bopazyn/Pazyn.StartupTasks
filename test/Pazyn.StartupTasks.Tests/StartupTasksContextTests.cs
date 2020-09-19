using Xunit;

namespace Pazyn.StartupTasks.Tests
{
    public class StartupTasksContextTests
    {
        [Fact]
        public void Context_With_No_Task()
        {
            var startupTasksContext = new StartupTasksContext();

            Assert.True(startupTasksContext.HaveAllTasksFinished);
            Assert.False(startupTasksContext.HasAnyTaskFailed);
        }

        [Fact]
        public void Context_Before_Tasks_Execution()
        {
            var startupTasksContext = new StartupTasksContext();
            startupTasksContext.RegisterTask();

            Assert.False(startupTasksContext.HaveAllTasksFinished);
            Assert.False(startupTasksContext.HasAnyTaskFailed);
        }

        [Fact]
        public void Context_During_Tasks_Execution()
        {
            var startupTasksContext = new StartupTasksContext();
            startupTasksContext.RegisterTask();
            startupTasksContext.RegisterTask();
            startupTasksContext.RegisterTask();

            startupTasksContext.MarkTaskAsComplete();

            Assert.False(startupTasksContext.HaveAllTasksFinished);
            Assert.False(startupTasksContext.HasAnyTaskFailed);
        }

        [Fact]
        public void Context_When_Task_Fails()
        {
            var startupTasksContext = new StartupTasksContext();
            startupTasksContext.RegisterTask();
            startupTasksContext.MarkTaskAsFailed();

            Assert.True(startupTasksContext.HaveAllTasksFinished);
            Assert.True(startupTasksContext.HasAnyTaskFailed);
        }

        [Fact]
        public void Context_After_Tasks_Execution()
        {
            var startupTasksContext = new StartupTasksContext();
            startupTasksContext.RegisterTask();
            startupTasksContext.RegisterTask();
            startupTasksContext.MarkTaskAsComplete();
            startupTasksContext.MarkTaskAsComplete();

            Assert.True(startupTasksContext.HaveAllTasksFinished);
            Assert.False(startupTasksContext.HasAnyTaskFailed);
        }
    }
}