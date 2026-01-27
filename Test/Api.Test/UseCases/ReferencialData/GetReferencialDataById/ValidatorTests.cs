using Application.UseCases.ReferencialData.GetReferencialDataById;
using FluentAssertions;

namespace Api.Test.UseCases.ReferencialData.GetReferencialDataById;

public class ValidatorTests
{
    [Fact]
    public void Validate_ValidRequest_ReturnsNoValidationErrors()
    {
        // Arrange
        var validator = new GetReferencialDataByIdValidator();
        var request = new GetReferencialDataByIdRequest { Id = Guid.NewGuid() };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyId_ReturnsValidationErrors()
    {
        // Arrange
        var validator = new GetReferencialDataByIdValidator();
        var request = new GetReferencialDataByIdRequest { Id = Guid.Empty };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(Constants.Constants.INVALID_ID_FORMAT));
    }

    [Fact]
    public void Validate_NullId_ReturnsValidationErrors()
    {
        // Arrange
        var validator = new GetReferencialDataByIdValidator();
        var request = new GetReferencialDataByIdRequest();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Equals(Constants.Constants.INVALID_ID_FORMAT));
    }
}