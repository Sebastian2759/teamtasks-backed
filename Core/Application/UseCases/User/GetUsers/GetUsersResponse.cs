using Domain.QueryModels;

namespace Application.UseCases.User.GetUsers;

public class GetUsersResponse
{
    public PagedResultQueryModel<UserListItemQueryModel> Page { get; set; } = new();
}
