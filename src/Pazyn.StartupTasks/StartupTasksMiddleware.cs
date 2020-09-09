using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Pazyn.StartupTasks
{
    internal class StartupTasksMiddleware
    {
        private StartupTaskContext Context { get; }
        private RequestDelegate Next { get; }

        public StartupTasksMiddleware(StartupTaskContext context, RequestDelegate next)
        {
            Context = context;
            Next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint != null && endpoint.Metadata.GetMetadata<StartupTaskMetadata>() == null)
            {
                await Next(httpContext);
                return;
            }

            if (Context.HaveAllTasksFinished)
            {
                if (Context.HasAnyTaskFailed)
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