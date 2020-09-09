using Xunit;

namespace Pazyn.StartupTasks.Tests
{
    public class StartupTaskContextTests
    {
        [Fact]
        public void Context_With_No_Task()
        {
            var startupTaskContext = new StartupTaskContext();

            Assert.True(startupTaskContext.HaveAllTasksFinished);
            Assert.False(startupTaskContext.HasAnyTaskFailed);
        }

        [Fact]
        public void Context_Before_Tasks_Execution()
        {
            var startupTaskContext = new StartupTaskContext();
            startupTaskContext.RegisterTask();

            Assert.False(startupTaskContext.HaveAllTasksFinished);
            Assert.False(startupTaskContext.HasAnyTaskFailed);
        }

        [Fact]
        public void Context_During_Tasks_Execution()
        {
            var startupTaskContext = new StartupTaskContext();
            startupTaskContext.RegisterTask();
            startupTaskContext.RegisterTask();
            startupTaskContext.RegisterTask();

            startupTaskContext.MarkTaskAsComplete();

            Assert.False(startupTaskContext.HaveAllTasksFinished);
            Assert.False(startupTaskContext.HasAnyTaskFailed);
        }

        [Fact]
        public void Context_When_Task_Fails()
        {
            var startupTaskContext = new StartupTaskContext();
            startupTaskContext.RegisterTask();
            startupTaskContext.MarkTaskAsFailed();

            Assert.True(startupTaskContext.HaveAllTasksFinished);
            Assert.True(startupTaskContext.HasAnyTaskFailed);
        }

        [Fact]
        public void Context_After_Tasks_Execution()
        {
            var startupTaskContext = new StartupTaskContext();
            startupTaskContext.RegisterTask();
            startupTaskContext.RegisterTask();
            startupTaskContext.MarkTaskAsComplete();
            startupTaskContext.MarkTaskAsComplete();

            Assert.True(startupTaskContext.HaveAllTasksFinished);
            Assert.False(startupTaskContext.HasAnyTaskFailed);
        }
    }
}