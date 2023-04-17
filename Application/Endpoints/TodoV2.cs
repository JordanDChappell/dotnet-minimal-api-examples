using Lib.Model.Generics;
using Lib.Model.Todo;
using Lib.Service.Todo;
using Lib.Validators;

namespace Application.Endpoints;

public static class TodoV2
{
  public static RouteGroupBuilder Register(RouteGroupBuilder builder)
  {
    builder
      .MapGet("/", async (TodoService service) => await service.GetAllTodosAsync())
      .WithName("GetTodosV2")
      .WithDescription("")
      .Produces<ItemsResponse<TodoResponse>>(StatusCodes.Status200OK)
      .MapToApiVersion(2);

    builder
      .MapGet("/{id}", async (int id, TodoService service) =>
        await service.GetTodoAsync(id) is TodoResponse response
            ? Results.Ok(response)
            : Results.NotFound())
      .WithName("GetTodoV2")
      .WithDescription("")
      .Produces<TodoResponse>(StatusCodes.Status200OK, "application/json")
      .Produces(StatusCodes.Status404NotFound)
      .MapToApiVersion(2);

    builder
      .MapPost("/", async (TodoRequest request, TodoService service) =>
        {
          var response = await service.CreateTodoAsync(request);
          return Results.Created($"/todos/{response.Id}", response);
        })
      .WithName("CreateTodoV2")
      .WithDescription("")
      .Accepts<TodoRequest>("application/json")
      .Produces(StatusCodes.Status201Created)
      .Produces(StatusCodes.Status400BadRequest)
      .AddEndpointFilter<ValidationFilter<TodoRequest>>()
      .MapToApiVersion(2);

    builder
      .MapPut("/{id}", async (int id, TodoRequest request, TodoService service) =>
        {
          if (await service.UpdateTodoAsync(id, request) is TodoResponse response)
            return Results.NoContent();
          return Results.NotFound();
        })
      .WithName("UpdateTodoV2")
      .WithDescription("")
      .Produces(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status400BadRequest)
      .AddEndpointFilter<ValidationFilter<TodoRequest>>()
      .MapToApiVersion(2);

    builder
      .MapDelete("/{id}", async (int id, TodoService service) =>
        {
          if (await service.DeleteTodoAsync(id) is TodoResponse response)
            return Results.NoContent();
          return Results.NotFound();
        })
      .WithName("DeleteTodoV2")
      .WithDescription("")
      .Produces(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status404NotFound)
      .MapToApiVersion(2);

    return builder;
  }
}