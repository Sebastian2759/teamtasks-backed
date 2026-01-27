using Application.Base;
using MediatR;

namespace Application.UseCases.ReferencialData.GetReferencialDataById;

public class GetReferencialDataByIdRequest : IRequest<ResponseBase<GetReferencialDataByIdResponse>>
{
    public Guid Id { get; set; }
}