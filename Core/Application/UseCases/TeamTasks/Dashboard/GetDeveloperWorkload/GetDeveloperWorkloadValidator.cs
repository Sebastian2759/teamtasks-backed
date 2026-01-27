using FluentValidation;

namespace Application.UseCases.TeamTasks.Dashboard.GetDeveloperWorkload;

public class GetDeveloperWorkloadValidator : AbstractValidator<GetDeveloperWorkloadRequest>
{
    public GetDeveloperWorkloadValidator() { }
}