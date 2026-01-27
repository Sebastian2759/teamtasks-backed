using Adapters.Mapper;
using Api.Test.Common;
using Application.UseCases.EconomicActivity.EconomicActivityAll;
using Domain.Contracts.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Api.Test.UseCases.EconomicActivity.EconomicActivityAll;

public class QueryTests
{
    [Fact]
    public async Task ValidQuery_ReturnsEconomicActivities()
    {
        // Arrange
        var economicActivities = GetTestEconomicActivities();

        var mockEconomicActivityRepository = new Mock<IEconomicActivityTypeRepository>();
        mockEconomicActivityRepository.Setup(repo => repo.GetEconomicActivityAllAsync())
                                .ReturnsAsync(economicActivities);

        var handler = new GetEconomicActivityAllQuery(mockEconomicActivityRepository.Object, new MapperAdapter());
        var query = new GetEconomicActivityAllRequest();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.EconomicActivitiesTypes.Should().HaveCount(economicActivities.Count());
    }

    private IEnumerable<EconomicActivityTypeEntity> GetTestEconomicActivities()
    {
        return JsonLoader.LoadJson<List<EconomicActivityTypeEntity>>("ResponseFiles/v1/responseGetEconomicActivities.json");
    }
}