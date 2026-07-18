using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using System.Linq;
using LibraryManagement.Business.Helpers.Exceptions; 

namespace LibraryManagement.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); 
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            object responseError;

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    responseError = new { message = notFoundEx.Message };
                    break;

                case ValidationException valEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = valEx.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage });
                    responseError = new { message = "Validation error", details = errors };
                    break;

                default:
                    responseError = new { message = "An unexpected system error occurred.", details = exception.Message };
                    break;
            }

            var result = JsonSerializer.Serialize(responseError);
            return context.Response.WriteAsync(result);
        }
    }
}