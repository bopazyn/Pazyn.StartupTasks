using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pazyn.StartupTasks.EntityFrameworkCore
{
    internal abstract class RunSqlStartupTask<T> : IStartupTask where T : DbContext
    {
        protected class Item
        {
            public String Name { get; set; }
            public Func<Stream> OpenStream { get; set; }
        }

        private T DbContext { get; }
        private Regex Pattern { get; }

        protected RunSqlStartupTask(T dbContext, Regex pattern)
        {
            DbContext = dbContext;
            Pattern = pattern;
        }

        public async Task<Boolean> Run(CancellationToken cancellationToken)
        {
            var items = GetItems().Where(x => Pattern.IsMatch(x.Name));
            foreach (var item in items)
            {
                await using var stream = item.OpenStream();
                using var streamReader = new StreamReader(stream);
                var sql = await streamReader.ReadToEndAsync();
                await DbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
            }

            return true;
        }

        protected abstract IEnumerable<Item> GetItems();
    }
}