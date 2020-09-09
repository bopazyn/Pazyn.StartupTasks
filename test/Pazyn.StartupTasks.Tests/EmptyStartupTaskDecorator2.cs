using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pazyn.StartupTasks.Tests
{
    public class EmptyStartupTaskDecorator2 : IStartupTask
    {
        private IStartupTask InnerStartupTask { get; }

        public EmptyStartupTaskDecorator2(IStartupTask innerStartupTask)
        {
            InnerStartupTask = innerStartupTask;
        }

        public Task<Boolean> Run(CancellationToken cancellationToken) => InnerStartupTask.Run(cancellationToken);
    }
}