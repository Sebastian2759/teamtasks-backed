using FluentValidation;
using System.Text.Json;

namespace Application.UseCases.TeamTasks.CreateTask;

public class CreateTaskValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AssignedUserId).NotEqual(Guid.Empty);

        When(x => x.PriorityId.HasValue, () =>
        {
            RuleFor(x => x.PriorityId!.Value).NotEqual(Guid.Empty);
        });

        When(x => !string.IsNullOrWhiteSpace(x.AdditionalInfo), () =>
        {
            RuleFor(x => x.AdditionalInfo!)
                .Must(BeValidJson)
                .WithMessage("AdditionalInfo must be valid JSON.");
        });
    }

    private static bool BeValidJson(string json)
    {
        try { JsonDocument.Parse(json); return true; }
        catch { return false; }
    }
}