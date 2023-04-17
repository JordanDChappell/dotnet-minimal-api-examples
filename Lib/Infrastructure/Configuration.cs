using System.Reflection;
using System.Text.Json.Serialization;
using Asp.Versioning;
using FluentValidation;
using Lib.Model.Todo;
using Lib.OpenApi;
using Lib.Repository;
using Lib.Service.Todo;
using Lib.Validators.Todo;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lib.Infrastructure;

public static class Configuration
{
  public static WebApplication ConfigureApplication(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    var services = builder.Services;
    services.ConfigureApiVersioning();
    services.ConfigureSwagger();
    services.ConfigureJsonOptions();
    services.ConfigureModelValidation();
    services.ConfigureDependencies();

    var serviceProvider = services.BuildServiceProvider();
    serviceProvider.SeedTodoRepository();

    var app = builder.Build();

    return app;
  }

  public static IApplicationBuilder ConfigureSwaggerUi(this WebApplication app) => app
    .UseSwagger()
    .UseSwaggerUI(opts =>
    {
      foreach (var description in app.DescribeApiVersions())
      {
        opts.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
      }
    });

  private static void ConfigureApiVersioning(this IServiceCollection services) => services
    .AddApiVersioning(opts =>
    {
      opts.AssumeDefaultVersionWhenUnspecified = true;
      opts.ReportApiVersions = true;
      opts.ApiVersionReader = new UrlSegmentApiVersionReader();
      opts.DefaultApiVersion = new ApiVersion(1);
    }).AddApiExplorer(opts =>
    {
      opts.GroupNameFormat = "'v'VVV";
      opts.SubstituteApiVersionInUrl = true;
    });

  private static void ConfigureSwagger(this IServiceCollection services) => services
    .AddEndpointsApiExplorer()
    .ConfigureOptions<ConfigureSwaggerOptions>()
    .AddSwaggerGen(opts =>
    {
      opts.OperationFilter<SwaggerDefaultValues>();
      opts.EnableAnnotations();
    });

  /// <summary>
  /// Note: Configuring two different JsonOptions instances here, one is used by .NET API framework and the other by
  /// swagger.
  /// </summary>
  /// <param name="services"></param>
  private static void ConfigureJsonOptions(this IServiceCollection services) => services
    .Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    {
      options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
    {
      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

  private static void ConfigureModelValidation(this IServiceCollection services) => services
    .AddTransient<IValidator<TodoRequest>, TodoRequestValidator>();

  private static IServiceCollection ConfigureDependencies(this IServiceCollection services) => services
    .ConfigureDatabase()
    .ConfigureServices();

  private static IServiceCollection ConfigureDatabase(this IServiceCollection services) => services
      .AddDbContext<TodoRepository>(opt => opt.UseInMemoryDatabase("TodoList"))
      .AddDatabaseDeveloperPageExceptionFilter();

  private static IServiceCollection ConfigureServices(this IServiceCollection services) => services
    .AddTransient<TodoService>();

  private static void SeedTodoRepository(this IServiceProvider serviceProvider)
  {
    var repository = serviceProvider.GetRequiredService<TodoRepository>();
    repository.AddRange(new List<TodoEntity>() {
      new TodoEntity() {
        Id = 1,
        Name = "Wash dishes",
        Status = TodoStatus.Incomplete,
        LastUpdateTsUtc = DateTime.Parse("2023-01-01T09:00:00").ToUniversalTime(),
      },
      new TodoEntity() {
        Id = 2,
        Name = "Make bed",
        Status = TodoStatus.Incomplete,
        LastUpdateTsUtc = DateTime.Parse("2023-01-01T10:00:00").ToUniversalTime(),
      },
      new TodoEntity() {
        Id = 3,
        Name = "Grocery shopping",
        Status = TodoStatus.Incomplete,
        LastUpdateTsUtc = DateTime.Parse("2023-01-01T10:07:00").ToUniversalTime(),
      },
    });
    repository.SaveChanges();
  }
}
