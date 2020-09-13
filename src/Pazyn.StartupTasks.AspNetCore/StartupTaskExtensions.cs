namespace Pazyn.StartupTasks.AspNetCore
{
    public static class StartupTaskExtensions
    {
        public static IStartupTaskBuilder AddOptionValidation<T>(this IStartupTaskBuilder startupTaskBuilder) where T : class, new() =>
            startupTaskBuilder.AddStartupTask<OptionValidationStartupTask<T>>();
    }
}