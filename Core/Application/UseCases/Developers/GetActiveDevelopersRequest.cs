using Application.Base;
using MediatR;

namespace Application.UseCases.Developers.GetActiveDevelopers;

public class GetActiveDevelopersRequest : IRequest<ResponseBase<GetActiveDevelopersResponse>>
{
}
