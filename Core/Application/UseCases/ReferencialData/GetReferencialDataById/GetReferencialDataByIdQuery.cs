using Application.Base;
using Application.Dtos.ReferencialData;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.ReferencialData.GetReferencialDataById;

public class GetReferencialDataByIdQuery(IReferencialDataRepository ReferencialData, IMapperAdapter mapper)
                              : IRequestHandler<GetReferencialDataByIdRequest, ResponseBase<GetReferencialDataByIdResponse>>
{
    private readonly IReferencialDataRepository _ReferencialData = ReferencialData;
    private readonly IMapperAdapter _mapperAdapter = mapper;

    public async Task<ResponseBase<GetReferencialDataByIdResponse>> Handle(GetReferencialDataByIdRequest request, CancellationToken cancellationToken)             
    {
        IEnumerable<ReferencialDataDetailsEntity> ReferencialData = await _ReferencialData.GetReferencialDataById(request.Id);
        ReferencialData = ReferencialData.OrderBy(x => x.Description);
         
        var response = new ResponseBase<GetReferencialDataByIdResponse>
        {
            Data = new GetReferencialDataByIdResponse
            {
                referencialDataDetails = _mapperAdapter.Map<ReferencialDataDetailsEntity, ReferencialDataDetailsDto>(ReferencialData)
            },
        };

        return response;
    }
}