using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pazyn.StartupTasks.Tests
{
    public class EmptyStartupTask1 : IStartupTask
    {
        public Task<Boolean> Run(CancellationToken cancellationToken) => Task.FromResult(true);
    }
}