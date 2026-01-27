using Application.Dtos.Developers;

namespace Application.UseCases.Developers.GetActiveDevelopers;

public class GetActiveDevelopersResponse
{
    public IEnumerable<DeveloperDto> Developers { get; set; } = [];
}
