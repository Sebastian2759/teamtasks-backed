using FluentValidation;

namespace Application.UseCases.TeamTasks.Dashboard.GetDeveloperDelayRisk;

public class GetDeveloperDelayRiskValidator : AbstractValidator<GetDeveloperDelayRiskRequest>
{
    public GetDeveloperDelayRiskValidator() { }
}