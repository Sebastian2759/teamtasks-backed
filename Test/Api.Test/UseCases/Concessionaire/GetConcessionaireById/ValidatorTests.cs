using Application.Enums;
using Application.UseCases.Concessionaire.GetConcessionaireById;
using FluentAssertions;
using static Application.Enums.Enums;

namespace Api.Test.UseCases.Concessionaire.GetConcessionaireById;

public class ValidatorTests
{
    [Fact]
    public void Validate_ValidRequest_ReturnsNoValidationErrors()
    {
        var company = Enums.Company.Autocolombiana;
        // Arrange
        var validator = new GetConcessionaireByIdValidator();
        var request = new GetConcessionaireByIdRequest { company = company };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidRequest_ReturnsValidationErrors()
    {
        var company = Enums.Company.Autocolombiana;
        // Arrange
        var validator = new GetConcessionaireByIdValidator();
        var request = new GetConcessionaireByIdRequest { company = company };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}