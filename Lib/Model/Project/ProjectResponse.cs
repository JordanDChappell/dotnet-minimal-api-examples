namespace Lib.Model.Project;

public class ProjectResponse
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public ProjectStatus Status { get; set; }
  public DateTime LastUpdateTsUtc { get; set; }

  public static ProjectResponse FromEntity(ProjectEntity entity) => new ProjectResponse()
  {
    Id = entity.Id,
    Name = entity.Name,
    Status = entity.Status,
    LastUpdateTsUtc = entity.LastUpdateTsUtc,
  };
}
