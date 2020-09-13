using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Pazyn.StartupTasks.EntityFrameworkCore
{
    internal class RunSqlFromDirectoryStartupTask<T> : RunSqlStartupTask<T> where T : DbContext
    {
        private DirectoryInfo DirectoryInfo { get; }

        public RunSqlFromDirectoryStartupTask(T dbContext, Regex pattern, DirectoryInfo directoryInfo) : base(dbContext, pattern)
        {
            DirectoryInfo = directoryInfo;
        }

        protected override IEnumerable<Item> GetItems()
        {
            var items = new List<Item>();

            void Process(DirectoryInfo directoryInfo, String prefix)
            {
                items.AddRange(directoryInfo.GetFiles()
                    .Select(x => new Item
                    {
                        Name = prefix + x.Name,
                        OpenStream = x.OpenRead
                    }));

                foreach (var di in directoryInfo.GetDirectories())
                {
                    Process(di, prefix + di.Name + "/");
                }
            }

            Process(DirectoryInfo, String.Empty);
            return items;
        }
    }
}