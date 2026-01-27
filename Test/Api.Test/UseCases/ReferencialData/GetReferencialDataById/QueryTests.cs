using Application.Dtos.ReferencialData;
using Application.UseCases.ReferencialData.GetReferencialDataById;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Api.Test.UseCases.ReferencialData.GetReferencialDataById;

public class QueryTests
{
    // Tests for the valid query scenario
    [Fact]
    public async Task ValidQuery_ReturnsReferencialData()
    {
        // Arrange
        var referencialDataDetails = GetTestReferencialDataDetails();

        var mockRepository = new Mock<IReferencialDataRepository>();
        mockRepository.Setup(repo => repo.GetReferencialDataById(It.IsAny<Guid>()))
                      .ReturnsAsync(referencialDataDetails);

        var mockMapper = new Mock<IMapperAdapter>();
        mockMapper.Setup(mapper => mapper.Map<IEnumerable<ReferencialDataDetailsEntity>, IEnumerable<ReferencialDataDetailsDto>>(It.IsAny<IEnumerable<ReferencialDataDetailsEntity>>()))
                  .Returns(GetTestReferencialDataDetailsDto());

        var handler = new GetReferencialDataByIdQuery(mockRepository.Object, mockMapper.Object);
        var request = new GetReferencialDataByIdRequest { Id = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.referencialDataDetails.Should().HaveCount(referencialDataDetails.Count());
    }

    // Tests for the invalid query scenario
    [Fact]
    public async Task InvalidQuery_ReturnsNotFound()
    {
        // Arrange
        var mockRepository = new Mock<IReferencialDataRepository>();
        mockRepository.Setup(repo => repo.GetReferencialDataById(It.IsAny<Guid>()))
                      .ReturnsAsync((IEnumerable<ReferencialDataDetailsEntity>)null);

        var mockMapper = new Mock<IMapperAdapter>();

        var query = new GetReferencialDataByIdQuery(mockRepository.Object, mockMapper.Object);
        var request = new GetReferencialDataByIdRequest { Id = Guid.NewGuid() };

        // Act
        var result = await query.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Message.Should().Be(Constants.Constants.REFER_DATA_NOT_FOUND);
        result.Data.Should().BeNull();
    }

    // Private helper methods for creating test data
    private IEnumerable<ReferencialDataDetailsEntity> GetTestReferencialDataDetails()
    {
        return new List<ReferencialDataDetailsEntity>
        {
            new ReferencialDataDetailsEntity { Id = Guid.NewGuid(), Description = "Test Description 1" },
            new ReferencialDataDetailsEntity { Id = Guid.NewGuid(), Description = "Test Description 2" }
        };
    }

    private IEnumerable<ReferencialDataDetailsDto> GetTestReferencialDataDetailsDto()
    {
        return new List<ReferencialDataDetailsDto>
        {
            new ReferencialDataDetailsDto { Id = Guid.NewGuid(), Description = "Test Description 1" },
            new ReferencialDataDetailsDto { Id = Guid.NewGuid(), Description = "Test Description 2" }
        };
    }
}