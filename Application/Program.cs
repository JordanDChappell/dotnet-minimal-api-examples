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
// This endpoint does not follow a consistent naming pattern and returns a flat array structure
todoItems
  .MapGet("/all", async (TodoService service) => await service.GetAllTodosAsync())
  .WithName("GetTodosV1")
  .MapToApiVersion(1);

// This endpoint is using a query parameter to fetch by a resource identifier - resources should be identified through
// their route. The resource does not make use of response types when returning content
todoItems
  .MapGet("/", async (int id, TodoService service) => await service.GetTodoAsync(id))
  .WithName("GetTodoV1")
  .MapToApiVersion(1);

// The POST request does not need to 
todoItems
  .MapPost("/create", async (TodoRequest request, TodoService service) => await service.CreateTodoAsync(request))
  .WithName("CreateTodoV1")
  .MapToApiVersion(1);

todoItems
  .MapPost("/update", async (int id, TodoRequest request, TodoService service) => await service.UpdateTodoAsync(id, request))
  .WithName("UpdateTodoV1")
  .MapToApiVersion(1);

todoItems
  .MapPost("/delete", async (int id, TodoService service) => await service.DeleteTodoAsync(id))
  .WithName("DeleteTodoV1")
  .MapToApiVersion(1);

// v2
todoItems
  .MapGet("/", async (TodoService service) =>
    new { items = await service.GetAllTodosAsync() })
  .WithName("GetTodosV2")
  .MapToApiVersion(2);

todoItems
  .MapGet("/{id}", async (int id, TodoService service) =>
    await service.GetTodoAsync(id) is TodoResponse response
        ? Results.Ok(response)
        : Results.NotFound())
  .WithName("GetTodoV2")
  .MapToApiVersion(2);

todoItems
  .MapPost("/", async (TodoRequest request, TodoService service) =>
    {
      var response = await service.CreateTodoAsync(request);
      return Results.Created($"/todos/{response.Id}", response);
    })
  .WithName("CreateTodoV2")
  .MapToApiVersion(2);

todoItems
  .MapPut("/{id}", async (int id, TodoRequest request, TodoService service) =>
    {
      if (await service.UpdateTodoAsync(id, request) is TodoResponse response)
        return Results.NoContent();
      return Results.NotFound();
    })
  .WithName("UpdateTodoV2")
  .MapToApiVersion(2);

todoItems
  .MapDelete("/{id}", async (int id, TodoService service) =>
    {
      if (await service.DeleteTodoAsync(id) is TodoResponse response)
        return Results.NoContent();
      return Results.NotFound();
    })
  .WithName("DeleteTodoV2")
  .MapToApiVersion(2);

app.ConfigureSwaggerUi();
app.Run();
