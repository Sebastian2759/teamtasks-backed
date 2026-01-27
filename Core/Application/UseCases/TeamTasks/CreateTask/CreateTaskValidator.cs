using FluentValidation;

namespace Application.UseCases.TeamTasks.CreateTask;

public class CreateTaskValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.")
            .NotEqual(Guid.Empty).WithMessage("Invalid ProjectId format.");

        RuleFor(x => x.AssigneeId)
            .NotEmpty().WithMessage("AssigneeId is required.")
            .NotEqual(Guid.Empty).WithMessage("Invalid AssigneeId format.");

        RuleFor(x => x.IdTaskStatus)
            .NotEmpty().WithMessage("IdTaskStatus is required.")
            .NotEqual(Guid.Empty).WithMessage("Invalid IdTaskStatus format.");

        RuleFor(x => x.IdTaskPriority)
            .NotEmpty().WithMessage("IdTaskPriority is required.")
            .NotEqual(Guid.Empty).WithMessage("Invalid IdTaskPriority format.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title max length is 200.");

        RuleFor(x => x.EstimatedComplexity)
            .InclusiveBetween(1, 5).WithMessage("EstimatedComplexity must be between 1 and 5.");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("DueDate is required.");
    }
}