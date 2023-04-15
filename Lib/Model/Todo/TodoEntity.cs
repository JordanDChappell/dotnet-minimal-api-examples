namespace Lib.Model.Todo;

public class TodoEntity
{
  public int Id { get; set; }
  public int? ProjectId { get; set; }
  public string? Name { get; set; }
  public TodoStatus Status { get; set; }
  public DateTime CreatedTsUtc { get; set; }
  public DateTime LastUpdateTsUtc { get; set; }
}