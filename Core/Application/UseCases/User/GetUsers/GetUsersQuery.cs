using Application.Base;
using Domain.Contracts.Persistence;
using MediatR;

namespace Application.UseCases.User.GetUsers;

public class GetUsersQuery(IUsersRepository usersRepository)
    : IRequestHandler<GetUsersRequest, ResponseBase<GetUsersResponse>>
{
    private readonly IUsersRepository _usersRepository = usersRepository;

    public async Task<ResponseBase<GetUsersResponse>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseBase<GetUsersResponse>();

        var page = await _usersRepository.GetUsersPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.IsActive,
            cancellationToken);

        response.Data = new GetUsersResponse { Page = page };
        return response;
    }
}