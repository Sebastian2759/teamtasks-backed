using FluentValidation;

namespace Application.UseCases.TeamTasks.GetProjectTasksPaged;

public class GetProjectTasksPagedValidator : AbstractValidator<GetProjectTasksPagedRequest>
{
    public GetProjectTasksPagedValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.")
            .NotEqual(Guid.Empty).WithMessage("Invalid ProjectId format.");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page must be >= 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize must be >= 1.")
            .LessThanOrEqualTo(100).WithMessage("PageSize must be <= 100.");
    }
}
