using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Pazyn.StartupTasks.AspNetCore
{
    public class OptionValidationStartupTask<T> : IStartupTask where T : class, new()
    {
        private IOptions<T> Options { get; }

        public OptionValidationStartupTask(IOptions<T> options)
        {
            Options = options;
        }

        public Task<Boolean> Run(CancellationToken cancellationToken)
        {
            // ReSharper disable once UnusedVariable
            var options = Options.Value;
            return Task.FromResult(true);
        }
    }
}