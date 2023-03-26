using Microsoft.EntityFrameworkCore;

using Lib.Infrastructure;
using Lib.Repository;
using Lib.Model.Todo;

var app = Configuration.ConfigureApplication(args);

var todoItems = app.MapGroup("/todos");

todoItems.MapGet("/", async (TodoRepository repo) =>
  new { items = await repo.Todos.ToListAsync() }
);
todoItems.MapGet("/{id}", async (int id, TodoRepository repo) =>
  await repo.Todos.FindAsync(id) is Todo todo
      ? Results.Ok(todo)
      : Results.NotFound()
);
todoItems.MapPost("/", async (TodoRequest request, TodoRepository repo) =>
{
  var todo = request.ToEntity();
  repo.Add(todo);
  await repo.SaveChangesAsync();
  return Results.Created($"/todos/{todo.Id}", todo);
});
todoItems.MapPut("/{id}", async (int id, TodoRequest request, TodoRepository repo) =>
{
  if (await repo.Todos.FindAsync(id) is Todo todo)
  {
    todo.Name = request.Name;
    todo.Status = request.Status;
    todo.LastUpdateTsUtc = DateTime.UtcNow;

    await repo.SaveChangesAsync();
    return Results.NoContent();
  }

  return Results.NotFound();
});
todoItems.MapDelete("/{id}", async (int id, TodoRepository repo) =>
{
  if (await repo.Todos.FindAsync(id) is Todo todo)
  {
    repo.Todos.Remove(todo);
    await repo.SaveChangesAsync();
    return Results.NoContent();
  }

  return Results.NotFound();
});

app.Run();
