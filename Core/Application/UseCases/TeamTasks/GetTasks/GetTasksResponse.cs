using Domain.QueryModels;

namespace Application.UseCases.TeamTasks.GetTasks;

public class GetTasksResponse
{
    public PagedResultQueryModel<TaskListItemQueryModel> Page { get; set; } = new();
}