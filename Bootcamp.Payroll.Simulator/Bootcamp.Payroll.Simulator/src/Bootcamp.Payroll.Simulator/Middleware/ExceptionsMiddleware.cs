using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Bootcamp.Payroll.Simulator.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionsMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            var controllerName = getRequestController(httpContext.Request.Path.Value) ?? string.Empty;
            var controllerMethodName = getRequestControllerMethod(httpContext.Request.Path.Value) ?? string.Empty;
            try
            {
                await this.next(httpContext);
            }
            catch (Exception exception)
            {
                httpContext.Response.StatusCode = 500;
                //await context.Response.WriteAsync(exception.Message);
                await httpContext.Response.WriteAsync(exception.Message);
            }
            
        }

        private string getRequestController(string apiPath)
        {
            var apiComps = apiPath.Split('/').ToList();
            var apiInd = apiComps.IndexOf("api");
            return apiComps[apiInd + 1];
        }

        private string getRequestControllerMethod(string apiPath)
        {
            var apiComps = apiPath.Split('/').ToList();
            var apiInd = apiComps.IndexOf("api");
            try
            {
                return apiComps[apiInd + 2];
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string getControllerName(string fullControllerName)
        {
            return fullControllerName.Replace("Controller", string.Empty);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionsMiddleware>();
        }
    }


}
