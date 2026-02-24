using Application.Base;
using MediatR;
using static Application.Enums.Enums;

namespace Application.UseCases.Values.GetValues;

public sealed class GetValuesRequest : IRequest<ResponseBase<GetValuesResponse>>
{
    public ValuesType Type { get; set; }
}
