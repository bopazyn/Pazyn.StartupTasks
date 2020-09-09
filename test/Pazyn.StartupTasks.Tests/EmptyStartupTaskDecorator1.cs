using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pazyn.StartupTasks.Tests
{
    public class EmptyStartupTaskDecorator1 : IStartupTask
    {
        private IStartupTask InnerStartupTask { get; }

        public EmptyStartupTaskDecorator1(IStartupTask innerStartupTask)
        {
            InnerStartupTask = innerStartupTask;
        }

        public Task<Boolean> Run(CancellationToken cancellationToken) => InnerStartupTask.Run(cancellationToken);
    }
}