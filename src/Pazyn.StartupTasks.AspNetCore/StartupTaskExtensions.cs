namespace Pazyn.StartupTasks.AspNetCore
{
    public static class StartupTaskExtensions
    {
        public static IStartupTasksBuilder AddOptionValidation<T>(this IStartupTasksBuilder startupTasksBuilder) where T : class, new() =>
            startupTasksBuilder.AddStartupTask<OptionValidationStartupTask<T>>();
    }
}