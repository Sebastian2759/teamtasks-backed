using Application.Base;
using Application.Dtos.Developers;
using Domain.Contracts.Persistence;
using MediatR;

namespace Application.UseCases.Developers.GetActiveDevelopers;

public class GetActiveDevelopersQuery(IDevelopersRepository developersRepo)
    : IRequestHandler<GetActiveDevelopersRequest, ResponseBase<GetActiveDevelopersResponse>>
{
    private readonly IDevelopersRepository _developersRepo = developersRepo;

    public async Task<ResponseBase<GetActiveDevelopersResponse>> Handle(GetActiveDevelopersRequest request, CancellationToken cancellationToken)
    {
        var devs = await _developersRepo.GetActiveDevelopers();

        return new ResponseBase<GetActiveDevelopersResponse>
        {
            Data = new GetActiveDevelopersResponse
            {
                Developers = devs.Select(d => new DeveloperDto
                {
                    DeveloperId = d.DeveloperId,
                    FullName = $"{d.FirstName} {d.LastName}".Trim(),
                    Email = d.Email
                })
            }
        };
    }
}
