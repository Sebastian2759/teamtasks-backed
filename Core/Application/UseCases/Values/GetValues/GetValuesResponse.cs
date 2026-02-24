using Application.Dtos;

namespace Application.UseCases.Values.GetValues;

public sealed class GetValuesResponse
{
    public IEnumerable<ValueItemDto> Items { get; set; } = Enumerable.Empty<ValueItemDto>();
}
