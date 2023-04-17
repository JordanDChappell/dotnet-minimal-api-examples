using Lib.Model.Todo;

namespace Lib.Contracts.Todo;

public class TodoRequest
{
  public string? Name { get; set; }
  public TodoStatus Status { get; set; }

  public TodoEntity ToEntity() => new TodoEntity()
  {
    Name = Name,
    Status = Status,
    CreatedTsUtc = DateTime.UtcNow,
    LastUpdateTsUtc = DateTime.UtcNow,
  };
}
