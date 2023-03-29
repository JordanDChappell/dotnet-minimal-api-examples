using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lib.OpenApi;
public class SwaggerDefaultValues : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    var apiDescription = context.ApiDescription;

    operation.Deprecated |= apiDescription.IsDeprecated();

    foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
    {
      var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
      var response = operation.Responses[responseKey];

      foreach (var contentType in response.Content.Keys)
      {
        if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
          response.Content.Remove(contentType);
      }
    }

    if (operation.Parameters == null)
      return;

    foreach (var parameter in operation.Parameters)
    {
      var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

      parameter.Description ??= description.ModelMetadata?.Description;

      if (parameter.Schema.Default == null && description.DefaultValue != null && description.ModelMetadata != null)
      {
        if (description.ModelMetadata.ModelType == typeof(int))
          description.DefaultValue = 0;

        var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
        parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
      }

      parameter.Required |= description.IsRequired;
    }
  }
}