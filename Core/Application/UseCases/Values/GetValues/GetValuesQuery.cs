using Application.Base;
using Application.Constants;
using Application.Dtos;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using MediatR;
using static Application.Enums.Enums;

namespace Application.UseCases.Values.GetValues;

public sealed class GetValuesQuery(IMasterDataDetailRepository repo, IMapperAdapter mapper)
    : IRequestHandler<GetValuesRequest, ResponseBase<GetValuesResponse>>
{
    private readonly IMasterDataDetailRepository _repo = repo;
    private readonly IMapperAdapter _mapper = mapper;

    public async Task<ResponseBase<GetValuesResponse>> Handle(GetValuesRequest request, CancellationToken ct)
    {
        var response = new ResponseBase<GetValuesResponse>();

        var masterId = Application.Constants.Constants.ValuesMasterIds[request.Type];

        var entities = await _repo.GetByMasterIdAsync(masterId, ct);

        var items = _mapper.Map<Domain.Entities.MasterDataDetailEntity, ValueItemDto>(entities).ToList();



        response.Data = new GetValuesResponse { Items = items };
        return response;
    }


}