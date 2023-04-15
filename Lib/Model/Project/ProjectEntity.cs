using Lib.Model.Todo;

namespace Lib.Model.Project;

public class ProjectEntity
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public ProjectStatus Status { get; set; }
  public IEnumerable<TodoEntity>? Todos { get; set; }
  public DateTime CreatedTsUtc { get; set; }
  public DateTime LastUpdateTsUtc { get; set; }
}