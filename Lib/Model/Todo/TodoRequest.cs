namespace Lib.Model.Todo;

public class TodoRequest
{
  public string? Name { get; set; }
  public TodoStatus Status { get; set; }

  public Todo ToEntity() => new Todo()
  {
    Name = Name,
    Status = Status,
    CreatedTsUtc = DateTime.UtcNow,
    LastUpdateTsUtc = DateTime.UtcNow,
  };
}
