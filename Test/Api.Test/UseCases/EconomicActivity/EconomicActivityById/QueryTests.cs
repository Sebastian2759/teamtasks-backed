using Adapters.Mapper;
using Api.Test.Common;
using Application.UseCases.EconomicActivity.GetEconomicActivityById;
using Domain.Contracts.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Api.Test.UseCases.EconomicActivity.EconomicActivityById;

public class QueryTests
{
    [Fact]
    public async Task ValidQuery_ReturnsEconomicActivityById()
    {
        // Arrange
        var economicActivity = GetTestEconomicActivity();
        var mockEconomicActivityRepository = new Mock<IEconomicActivityTypeRepository>();
        mockEconomicActivityRepository.Setup(repo => repo.GetEconomicActivityByIdAsync(Constants.Constants.ID_ECONOMIC_ACTIVITY))
                                      .ReturnsAsync(economicActivity);

        var handler = new GetEconomicActivityByIdQuery(mockEconomicActivityRepository.Object, new MapperAdapter());
        var query = new GetEconomicActivityByIdRequest { Id = Constants.Constants.ID_ECONOMIC_ACTIVITY };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.EconomicActivityType.Should().BeEquivalentTo(economicActivity);
    }

    private EconomicActivityTypeEntity GetTestEconomicActivity()
    {
        var financialEntities = JsonLoader.LoadJson<List<EconomicActivityTypeEntity>>("ResponseFiles/v1/responseGetEconomicActivityById.json");

        return financialEntities.FirstOrDefault(fe => fe.Code == Constants.Constants.ID_ECONOMIC_ACTIVITY);
    }
}