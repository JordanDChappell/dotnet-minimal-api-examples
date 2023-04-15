namespace Lib.Model.Project;

public class ProjectRequest
{
  public string? Name { get; set; }
  public ProjectStatus Status { get; set; }

  public ProjectEntity ToEntity() => new ProjectEntity()
  {
    Name = Name,
    Status = Status,
    CreatedTsUtc = DateTime.UtcNow,
    LastUpdateTsUtc = DateTime.UtcNow,
  };
}
