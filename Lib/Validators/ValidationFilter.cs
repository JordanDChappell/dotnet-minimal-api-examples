using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Lib.Validators;

public class ValidationFilter<T> : IEndpointFilter
{
  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
  {
    T? argToValidate = context.GetArgument<T>(0);
    IValidator<T>? validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

    if (validator is not null)
    {
      var validationResult = await validator.ValidateAsync(argToValidate!);
      if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.BadRequest);
    }

    // Otherwise invoke the next filter in the pipeline
    return await next.Invoke(context);
  }
}