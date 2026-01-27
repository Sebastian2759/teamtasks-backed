using FluentAssertions;
using Application.UseCases.Department.GetDepartmentByCountryId;

namespace Api.Test.UseCases.Department.GetDepartmentByCountryId;

public class ValidatorTests
{
    [Fact]
    public void Validate_ValidRequest_ReturnsNoValidationErrors()
    {
        // Arrange
        var validator = new GetDepartmentByCountryIdValidator();
        var request = new GetDepartmentByCountryIdRequest { CountryId = 1 };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidRequest_ReturnsValidationErrors()
    {
        // Arrange
        var validator = new GetDepartmentByCountryIdValidator();
        var request = new GetDepartmentByCountryIdRequest { CountryId = -1 };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}