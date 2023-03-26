using Microsoft.EntityFrameworkCore;

using Lib.Model.Todo;
using Lib.Repository;

namespace Lib.Service.Todo;

public class TodoService
{
  private readonly TodoRepository _repository;

  public TodoService(TodoRepository repository)
  {
    _repository = repository;
  }

  public async Task<IEnumerable<TodoResponse>> GetAllTodosAsync()
  {
    var todos = await _repository.Todos.ToListAsync();
    return todos.Select(TodoResponse.FromEntity);
  }

  public async Task<TodoResponse?> GetTodoAsync(int id)
  {
    var todo = await _repository.Todos.FindAsync(id);

    if (todo is null)
      return null;

    return TodoResponse.FromEntity(todo);
  }

  public async Task<TodoResponse> CreateTodoAsync(TodoRequest request)
  {
    var todo = request.ToEntity();
    _repository.Add(todo);
    await _repository.SaveChangesAsync();
    return TodoResponse.FromEntity(todo);
  }

  public async Task<TodoResponse?> UpdateTodoAsync(int id, TodoRequest request)
  {
    var todo = await _repository.Todos.FindAsync(id);

    if (todo is null)
      return null;

    todo.Name = request.Name;
    todo.Status = request.Status;
    todo.LastUpdateTsUtc = DateTime.UtcNow;

    await _repository.SaveChangesAsync();
    return TodoResponse.FromEntity(todo);
  }

  public async Task<TodoResponse?> DeleteTodoAsync(int id)
  {
    var todo = await _repository.Todos.FindAsync(id);

    if (todo is null)
      return null;

    _repository.Remove(todo);
    await _repository.SaveChangesAsync();
    return TodoResponse.FromEntity(todo);
  }
}