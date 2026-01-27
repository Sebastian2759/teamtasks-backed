using Application.Dtos.TeamTasks;

namespace Application.UseCases.TeamTasks.GetProjectTasksPaged;

public class GetProjectTasksPagedResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<TaskDto> Tasks { get; set; } = new List<TaskDto>();
}
