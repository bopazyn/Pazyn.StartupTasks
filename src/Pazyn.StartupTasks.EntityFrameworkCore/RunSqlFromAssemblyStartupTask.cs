using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Pazyn.StartupTasks.EntityFrameworkCore
{
    internal class RunSqlFromAssemblyStartupTask<T> : RunSqlStartupTask<T> where T : DbContext
    {
        private Assembly Assembly { get; }

        public RunSqlFromAssemblyStartupTask(T dbContext, Regex pattern, Assembly assembly) : base(dbContext, pattern)
        {
            Assembly = assembly;
        }

        protected override IEnumerable<Item> GetItems() =>
            Assembly.GetManifestResourceNames()
                .Select(x => new Item
                {
                    Name = x.Substring(Assembly.GetName().Name.Length + 1),
                    OpenStream = () => Assembly.GetManifestResourceStream(x)
                });
    }
}