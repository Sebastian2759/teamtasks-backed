using Application.Base;
using MediatR;

namespace Application.UseCases.User.GetUsers;

public class GetUsersRequest : IRequest<ResponseBase<GetUsersResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
    public bool? IsActive { get; set; } = true;
}
