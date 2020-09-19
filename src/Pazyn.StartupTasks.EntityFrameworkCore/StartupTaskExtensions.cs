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
        public static IStartupTasksBuilder AddMigrateDatabaseStartupTask<T>(this IStartupTasksBuilder startupTasksBuilder) where T : DbContext =>
            startupTasksBuilder.AddStartupTask<MigrateDatabaseStartupTask<T>>();

        public static IStartupTasksBuilder AddRunSqlFromDirectoryStartupTask<T>(this IStartupTasksBuilder startupTasksBuilder, String directoryPath, Regex pattern = null) where T : DbContext =>
            startupTasksBuilder.AddStartupTask(sp => ActivatorUtilities.CreateInstance<RunSqlFromDirectoryStartupTask<T>>(sp, new DirectoryInfo(directoryPath), pattern ?? new Regex(".*")), sti => sti.DisplayName = $"{nameof(RunSqlFromDirectoryStartupTask<T>)} {directoryPath} {pattern}");

        public static IStartupTasksBuilder AddRunSqlFromAssemblyStartupTask<T>(this IStartupTasksBuilder startupTasksBuilder, Assembly assembly, Regex pattern = null) where T : DbContext =>
            startupTasksBuilder.AddStartupTask(sp => ActivatorUtilities.CreateInstance<RunSqlFromAssemblyStartupTask<T>>(sp, assembly, pattern ?? new Regex(".*")), sti => sti.DisplayName = $"{nameof(RunSqlFromAssemblyStartupTask<T>)} {assembly} {pattern}");
    }
}
