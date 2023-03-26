using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Lib.Model.Todo;
using Lib.Repository;
using Lib.Service.Todo;

namespace Lib.Infrastructure
{
  public static class Configuration
  {
    public static WebApplication ConfigureApplication(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      builder.Services.ConfigureDependencies();

      var serviceProvider = builder.Services.BuildServiceProvider();
      serviceProvider.SeedTodoRepository();

      return builder.Build();
    }

    public static IServiceCollection ConfigureDependencies(this IServiceCollection services) => services
        .AddDbContext<TodoRepository>(opt => opt.UseInMemoryDatabase("TodoList"))
        .AddDatabaseDeveloperPageExceptionFilter()
        .AddTransient<TodoService>();

    public static void SeedTodoRepository(this IServiceProvider serviceProvider)
    {
      var repository = serviceProvider.GetRequiredService<TodoRepository>();
      repository.AddRange(new List<Todo>() {
        new Todo() {
          Id = 1,
          Name = "Wash dishes",
          Status = TodoStatus.Incomplete,
          LastUpdateTsUtc = DateTime.Parse("2023-01-01T09:00:00").ToUniversalTime(),
        },
        new Todo() {
          Id = 2,
          Name = "Make bed",
          Status = TodoStatus.Incomplete,
          LastUpdateTsUtc = DateTime.Parse("2023-01-01T10:00:00").ToUniversalTime(),
        },
        new Todo() {
          Id = 3,
          Name = "Grocery shopping",
          Status = TodoStatus.Incomplete,
          LastUpdateTsUtc = DateTime.Parse("2023-01-01T10:07:00").ToUniversalTime(),
        },
      });
      repository.SaveChanges();
    }
  }
}
