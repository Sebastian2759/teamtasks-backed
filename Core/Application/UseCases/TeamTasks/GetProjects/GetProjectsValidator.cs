using FluentValidation;

namespace Application.UseCases.TeamTasks.GetProjects;

public class GetProjectsValidator : AbstractValidator<GetProjectsRequest>
{
    public GetProjectsValidator()
    {
        // No params, no rules
    }
}