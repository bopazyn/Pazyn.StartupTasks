using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pazyn.StartupTasks.EntityFrameworkCore
{
    internal class MigrateDatabaseStartupTask<T> : IStartupTask where T : DbContext
    {
        private T DbContext { get; }

        public MigrateDatabaseStartupTask(T dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Boolean> Run(CancellationToken cancellationToken)
        {
            await DbContext.Database.MigrateAsync(cancellationToken);
            return true;
        }
    }
}