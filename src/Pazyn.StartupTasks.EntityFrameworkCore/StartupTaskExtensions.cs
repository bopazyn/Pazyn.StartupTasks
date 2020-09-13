using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Pazyn.StartupTasks.EntityFrameworkCore
{
    public static class StartupTaskExtensions
    {
        public static IStartupTaskBuilder AddMigrateDatabaseStartupTask<T>(this IStartupTaskBuilder startupTaskBuilder) where T : DbContext =>
            startupTaskBuilder.AddStartupTask<MigrateDatabaseStartupTask<T>>();

        public static IStartupTaskBuilder AddRunSqlFromDirectoryStartupTask<T>(this IStartupTaskBuilder startupTaskBuilder, String directoryPath, Regex pattern = null) where T : DbContext =>
            startupTaskBuilder.AddStartupTask(sp => ActivatorUtilities.CreateInstance<RunSqlFromDirectoryStartupTask<T>>(sp, new DirectoryInfo(directoryPath), pattern ?? new Regex(".*")), sti => sti.DisplayName = $"{nameof(RunSqlFromDirectoryStartupTask<T>)} {directoryPath} {pattern}");

        public static IStartupTaskBuilder AddRunSqlFromAssemblyStartupTask<T>(this IStartupTaskBuilder startupTaskBuilder, Assembly assembly, Regex pattern = null) where T : DbContext =>
            startupTaskBuilder.AddStartupTask(sp => ActivatorUtilities.CreateInstance<RunSqlFromAssemblyStartupTask<T>>(sp, assembly, pattern ?? new Regex(".*")), sti => sti.DisplayName = $"{nameof(RunSqlFromAssemblyStartupTask<T>)} {assembly} {pattern}");
    }
}
