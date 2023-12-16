using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PhotoZoneBooking.BLL.Exceptions;

namespace PhotoZoneBooking.BLL.Middlewares;

public class ErrorMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorMiddleware(RequestDelegate next)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        object errors = null;

        switch (ex)
        {
            case HttpException rest:
                errors = rest.Errors;
                context.Response.StatusCode = (int)rest.Code;
                break;

            case Exception e:
                errors = string.IsNullOrWhiteSpace(e.Message) ? "error" : e.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.ContentType = "application/json";

        if (errors != null)
        {
            var result = JsonConvert.SerializeObject(new
            {
                errors
            });

            await context.Response.WriteAsync(result);
        }
    }
}