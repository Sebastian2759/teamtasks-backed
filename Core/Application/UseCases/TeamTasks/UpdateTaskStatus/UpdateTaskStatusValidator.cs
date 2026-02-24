using FluentValidation;

namespace Application.UseCases.TeamTasks.UpdateTaskStatus;

public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusRequest>
{
    public UpdateTaskStatusValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("TaskId is required.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.");
    }
}
