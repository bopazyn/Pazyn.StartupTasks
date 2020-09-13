using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Pazyn.StartupTasks
{
    internal class StartupTasksMiddleware
    {
        private IOptions<StartupTaskContext> Context { get; }
        private RequestDelegate Next { get; }

        public StartupTasksMiddleware(IOptions<StartupTaskContext> context, RequestDelegate next)
        {
            Context = context;
            Next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var startupTaskContext = Context.Value;
            return ProcessRequest(httpContext, startupTaskContext);
        }

        public async Task ProcessRequest(HttpContext httpContext, StartupTaskContext startupTaskContext)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint != null && endpoint.Metadata.GetMetadata<StartupTaskMetadata>() == null)
            {
                await Next(httpContext);
                return;
            }

            if (startupTaskContext.HaveAllTasksFinished)
            {
                if (startupTaskContext.HasAnyTaskFailed)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
                else
                {
                    await Next(httpContext);
                }
            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                httpContext.Response.Headers[HeaderNames.RetryAfter] = "30";
            }
        }
    }
}