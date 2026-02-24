using FluentValidation;

namespace Application.UseCases.Values.GetValues;

public sealed class GetValuesValidator : AbstractValidator<GetValuesRequest>
{
    public GetValuesValidator()
    {
        RuleFor(x => x.Type)
            .Must(t => Application.Constants.Constants.ValuesMasterIds.ContainsKey(t))
            .WithMessage("Invalid values type.");
    }
}
