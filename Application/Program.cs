using Lib.Infrastructure;
using Lib.Model.Todo;
using Lib.Service.Todo;

var app = Configuration.ConfigureApplication(args);
var todo = app.NewVersionedApi("todos");
var todoItems = todo.MapGroup("/api/v{version:apiVersion}/todos")
  .WithName("Todos")
  .HasApiVersion(1)
  .HasApiVersion(2);

// v1
todoItems
  .MapGet("/all", async (TodoService service) =>
    new { items = await service.GetAllTodosAsync() })
  .WithName("GetTodosV1")
  .MapToApiVersion(1);

todoItems
  .MapGet("/{id}", async (int id, TodoService service) =>
    await service.GetTodoAsync(id) is TodoResponse response
        ? Results.Ok(response)
        : Results.NotFound())
  .WithName("GetTodo")
  .MapToApiVersion(1);

todoItems
  .MapPost("/", async (TodoRequest request, TodoService service) =>
    {
      var response = await service.CreateTodoAsync(request);
      return Results.Created($"/todos/{response.Id}", response);
    })
  .WithName("CreateTodo")
  .MapToApiVersion(1);


todoItems
  .MapPut("/{id}", async (int id, TodoRequest request, TodoService service) =>
    {
      if (await service.UpdateTodoAsync(id, request) is TodoResponse response)
        return Results.NoContent();
      return Results.NotFound();
    })
  .WithName("UpdateTodo")
  .MapToApiVersion(1);

todoItems
  .MapDelete("/{id}", async (int id, TodoService service) =>
    {
      if (await service.DeleteTodoAsync(id) is TodoResponse response)
        return Results.NoContent();
      return Results.NotFound();
    })
  .WithName("DeleteTodo")
  .MapToApiVersion(1);


// v2
todoItems
  .MapGet("/", async (TodoService service) =>
    new { items = await service.GetAllTodosAsync() })
  .WithName("GetTodosV2")
  .MapToApiVersion(2);

app.ConfigureSwaggerUi();
app.Run();
