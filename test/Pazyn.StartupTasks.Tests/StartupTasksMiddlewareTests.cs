using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Pazyn.StartupTasks.Tests
{
    public class StartupTasksMiddlewareTests
    {
        [Fact]
        public async Task Middleware_Returns_503_When_Tasks_Have_Not_Started()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            
            var startupTaskContext = new StartupTaskContext();
            startupTaskContext.RegisterTask();
            startupTaskContext.RegisterTask();

            var middleware = new StartupTasksMiddleware(null, httpContext => httpContext.Response.WriteAsync("Hello world!"));
            await middleware.ProcessRequest(context, startupTaskContext);

            Assert.Equal(503, context.Response.StatusCode);
        }

        [Fact]
        public async Task Middleware_Returns_500_When_Any_Task_Failed()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var startupTaskContext = new StartupTaskContext();
            startupTaskContext.RegisterTask();
            startupTaskContext.RegisterTask();
            startupTaskContext.MarkTaskAsComplete();
            startupTaskContext.MarkTaskAsFailed();

            var middleware = new StartupTasksMiddleware(null, httpContext => httpContext.Response.WriteAsync("Hello world!"));
            await middleware.ProcessRequest(context, startupTaskContext);

            Assert.Equal(500, context.Response.StatusCode);
        }

        [Fact]
        public async Task Middleware_Returns_Next_Middleware_When_All_Tasks_Completed()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var startupTaskContext = new StartupTaskContext();
            startupTaskContext.RegisterTask();
            startupTaskContext.RegisterTask();
            startupTaskContext.MarkTaskAsComplete();
            startupTaskContext.MarkTaskAsComplete();

            var middleware = new StartupTasksMiddleware(null, httpContext => httpContext.Response.WriteAsync("Hello world!"));
            await middleware.ProcessRequest(context, startupTaskContext);

            Assert.Equal(200, context.Response.StatusCode);
        }
    }
}
