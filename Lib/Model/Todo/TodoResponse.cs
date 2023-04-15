namespace Lib.Model.Todo;

public class TodoResponse
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public TodoStatus Status { get; set; }
  public DateTime LastUpdateTsUtc { get; set; }

  public static TodoResponse FromEntity(TodoEntity entity) => new TodoResponse()
  {
    Id = entity.Id,
    Name = entity.Name,
    Status = entity.Status,
    LastUpdateTsUtc = entity.LastUpdateTsUtc,
  };
}
