using Lib.Infrastructure;
using Lib.Model.Todo;
using Lib.Service.Todo;

var app = Configuration.ConfigureApplication(args);

var todoItems = app.MapGroup("/todos");

todoItems.MapGet("/", async (TodoService service) =>
  new { items = await service.GetAllTodosAsync() }
);
todoItems.MapGet("/{id}", async (int id, TodoService service) =>
  await service.GetTodoAsync(id) is TodoResponse response
      ? Results.Ok(response)
      : Results.NotFound()
);
todoItems.MapPost("/", async (TodoRequest request, TodoService service) =>
{
  var response = await service.CreateTodoAsync(request);
  return Results.Created($"/todos/{response.Id}", response);
});
todoItems.MapPut("/{id}", async (int id, TodoRequest request, TodoService service) =>
{
  if (await service.UpdateTodoAsync(id, request) is TodoResponse response)
    return Results.NoContent();
  return Results.NotFound();
});
todoItems.MapDelete("/{id}", async (int id, TodoService service) =>
{
  if (await service.DeleteTodoAsync(id) is TodoResponse response)
    return Results.NoContent();
  return Results.NotFound();
});

app.Run();
