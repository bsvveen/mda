using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace MDA.Infrastructure
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {  
                switch (error)
                {
                    case ModelException e:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(e.Message??"Error");
                        break;
                    case ModelValidationException e:                        
                        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(e.Modelstate) ?? "Error");
                        break;
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(error.Message ?? "Error");
                        break;
                }
            }           
        }

        public class ModelException : Exception
        {
            public ModelException() : base() { }

            public ModelException(string message) : base(message) { }

            public ModelException(string message, params object[] args)
                : base(String.Format(CultureInfo.CurrentCulture, message, args))
            {}
        }

        public class ModelValidationException : Exception
        {  
            public ModelValidationException(ModelStateDictionary modelstate) : base("Validation Error") {
                Modelstate = new SerializableError(modelstate);
            }

            public SerializableError Modelstate { get; set; }
        }        
    }
}
