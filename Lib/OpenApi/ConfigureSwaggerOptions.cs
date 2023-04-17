using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lib.OpenApi;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
  readonly IApiVersionDescriptionProvider _provider;

  public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

  public void Configure(SwaggerGenOptions options)
  {
    foreach (var description in _provider.ApiVersionDescriptions)
    {
      options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
    }
  }

  private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
  {
    var info = new OpenApiInfo()
    {
      Title = "Example minimal API",
      Version = description.ApiVersion.ToString(),
      Description = "A sample application to show off web API best practices.",
      Contact = new OpenApiContact() { Name = "Jordan Chappell", Email = "me@jordanchappell.com" },
      License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
    };

    if (description.IsDeprecated)
      info.Description += " This API version has been deprecated.";

    return info;
  }
}