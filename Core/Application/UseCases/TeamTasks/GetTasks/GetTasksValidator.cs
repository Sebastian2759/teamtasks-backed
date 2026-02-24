using FluentValidation;

namespace Application.UseCases.TeamTasks.GetTasks;

public class GetTasksValidator : AbstractValidator<GetTasksRequest>
{
    public GetTasksValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);

        When(x => x.AssignedUserId.HasValue, () =>
        {
            RuleFor(x => x.AssignedUserId!.Value).NotEqual(Guid.Empty);
        });

        When(x => x.PriorityId.HasValue, () =>
        {
            RuleFor(x => x.PriorityId!.Value).NotEqual(Guid.Empty);
        });

        When(x => !string.IsNullOrWhiteSpace(x.Status), () =>
        {
            RuleFor(x => x.Status!)
                .Must(s => Enum.TryParse<TaskStatus>(s, true, out _))
                .WithMessage("Status must be Pending, InProgress or Done.");
        });

        RuleFor(x => x.Search).MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.Search));
        RuleFor(x => x.Tag).MaximumLength(80).When(x => !string.IsNullOrWhiteSpace(x.Tag));
    }
}
