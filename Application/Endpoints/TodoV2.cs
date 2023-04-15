using Lib.Model.Todo;
using Lib.Service.Todo;

namespace Application.Endpoints;

public static class TodoV2
{
  public static RouteGroupBuilder Register(RouteGroupBuilder builder)
  {
    builder
      .MapGet("/", async (TodoService service) =>
        new { items = await service.GetAllTodosAsync() })
      .WithName("GetTodosV2")
      .MapToApiVersion(2);

    builder
      .MapGet("/{id}", async (int id, TodoService service) =>
        await service.GetTodoAsync(id) is TodoResponse response
            ? Results.Ok(response)
            : Results.NotFound())
      .WithName("GetTodoV2")
      .MapToApiVersion(2);

    builder
      .MapPost("/", async (TodoRequest request, TodoService service) =>
        {
          var response = await service.CreateTodoAsync(request);
          return Results.Created($"/todos/{response.Id}", response);
        })
      .WithName("CreateTodoV2")
      .MapToApiVersion(2);

    builder
      .MapPut("/{id}", async (int id, TodoRequest request, TodoService service) =>
        {
          if (await service.UpdateTodoAsync(id, request) is TodoResponse response)
            return Results.NoContent();
          return Results.NotFound();
        })
      .WithName("UpdateTodoV2")
      .MapToApiVersion(2);

    builder
      .MapDelete("/{id}", async (int id, TodoService service) =>
        {
          if (await service.DeleteTodoAsync(id) is TodoResponse response)
            return Results.NoContent();
          return Results.NotFound();
        })
      .WithName("DeleteTodoV2")
      .MapToApiVersion(2);

    return builder;
  }
}