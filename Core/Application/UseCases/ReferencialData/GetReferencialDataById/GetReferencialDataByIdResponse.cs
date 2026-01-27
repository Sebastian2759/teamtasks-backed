using Application.Dtos.ReferencialData;

namespace Application.UseCases.ReferencialData.GetReferencialDataById;

public class GetReferencialDataByIdResponse
{
    public IEnumerable<ReferencialDataDetailsDto> referencialDataDetails {  get; set; } 
}