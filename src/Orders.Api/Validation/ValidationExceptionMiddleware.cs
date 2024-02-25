using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Orders.Api.Validation;

public class ValidationExceptionMiddleware(RequestDelegate request)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await request(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = 400;
            
            var error = new ValidationProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = 400,
                Extensions =
                {
                    ["traceId"] = context.TraceIdentifier
                }
            };
            foreach (var validationFailure in exception.Errors)
            {
                error.Errors.Add(new KeyValuePair<string, string[]>(
                    validationFailure.PropertyName, 
                    new[] { validationFailure.ErrorMessage }));
            }
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}