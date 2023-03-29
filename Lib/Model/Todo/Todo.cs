namespace Lib.Model.Todo;

public enum TodoStatus
{
  Incomplete,
  Complete,
  Archived
}

public class Todo
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public TodoStatus Status { get; set; }
  public DateTime CreatedTsUtc { get; set; }
  public DateTime LastUpdateTsUtc { get; set; }
}
