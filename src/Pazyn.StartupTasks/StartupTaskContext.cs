using System;

namespace Pazyn.StartupTasks
{
    public class StartupTaskContext
    {
        private Object Object { get; } = new Object();
        private Int32 RegisteredStartupTasksCount { get; set; }
        private Int32 CompletedStartupTasksCount { get; set; }
        private Int32 FailedStartupTasksCount { get; set; }

        public void RegisterTask()
        {
            lock (Object)
            {
                RegisteredStartupTasksCount += 1;
            }
        }

        public void MarkTaskAsComplete()
        {
            lock (Object)
            {
                CompletedStartupTasksCount += 1;
            }
        }

        public void MarkTaskAsFailed()
        {
            lock (Object)
            {
                FailedStartupTasksCount += 1;
            }
        }

        public Boolean HaveAllTasksFinished =>
            RegisteredStartupTasksCount == CompletedStartupTasksCount + FailedStartupTasksCount;

        public Boolean HasAnyTaskFailed => FailedStartupTasksCount > 0;
    }
}