namespace Lib.Model.Todo;

public class TodoRequest
{
  public string? Name { get; set; }
  public TodoStatus Status { get; set; }

  public Todo ToEntity() => new Todo()
  {
    Name = Name,
    Status = Status,
    LastUpdateTsUtc = DateTime.UtcNow,
  };
}

