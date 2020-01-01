using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Synergy.Samples.Web.API.Extensions
{
    // TODO: Add swagger filter that shows the possible error 
    public class ExceptionHandlingMiddleware
    {
        private RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorId = Guid.NewGuid().ToString().Replace("-", "");
            // TODO: Log the error id so it would be esier to find the exception occurence when someone sends the id

            // TODO: Depending on exception type decide to return (or not) exception details to the client
            var details = new ErrorDetails(errorId, exception.Message);

            // TODO: Depending on exception type change returned status code
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var payload = JsonConvert.SerializeObject(details);
            return context.Response.WriteAsync(payload);
        }

        private class ErrorDetails
        {
            [JsonConstructor]
            public ErrorDetails(string errorId, string message)
            {
                ErrorId = errorId;
                Message = message;
            }

            [JsonProperty("message")]
            public string Message { get; }

            [JsonProperty("errorId")]
            public string ErrorId { get; }
        }
    }
}
