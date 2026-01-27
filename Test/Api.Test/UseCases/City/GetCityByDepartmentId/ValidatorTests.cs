using Application.UseCases.City.GetCityByDepartmentId;
using FluentAssertions;

namespace Api.Test.UseCases.City.GetCityByDepartmentId;

public class ValidatorTests
{
    [Fact]
    public void Validate_ValidRequest_ReturnsNoValidationErrors()
    {
        // Arrange
        var validator = new GetCityByDepartmentIdValidator();
        var request = new GetCityByDepartmentIdRequest { DepartmentId = "25" };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidRequest_ReturnsValidationErrors()
    {
        // Arrange
        var validator = new GetCityByDepartmentIdValidator();
        var request = new GetCityByDepartmentIdRequest { DepartmentId = "" };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}