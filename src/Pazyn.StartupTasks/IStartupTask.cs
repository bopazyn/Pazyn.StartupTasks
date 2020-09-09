using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pazyn.StartupTasks
{
    public interface IStartupTask
    {
        Task<Boolean> Run(CancellationToken cancellationToken);
    }
}