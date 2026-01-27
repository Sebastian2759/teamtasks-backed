using FluentValidation;

namespace Application.UseCases.TeamTasks.Dashboard.GetProjectHealth
{
    public class GetProjectHealthValidator : AbstractValidator<GetProjectHealthRequest>
    {
        public GetProjectHealthValidator() { }
    }
}