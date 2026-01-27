using FluentValidation;

namespace Application.UseCases.ReferencialData.GetReferencialDataById

{
    public class GetReferencialDataByIdValidator : AbstractValidator<GetReferencialDataByIdRequest>
    {
        public GetReferencialDataByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .NotEqual(Guid.Empty).WithMessage("Invalid Id format.");
        }
    }
}