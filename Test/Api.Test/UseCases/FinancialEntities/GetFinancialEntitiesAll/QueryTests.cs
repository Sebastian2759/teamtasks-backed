using Adapters.Mapper;
using Api.Test.Common;
using Application.UseCases.FinancialEntities.GetFinancialEntitiesAll;
using Domain.Contracts.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Api.Test.UseCases.FinancialEntities.GetFinancialEntitiesAll;

public class QueryTests
{
    [Fact]
    public async Task ValidQuery_ReturnsFinancialEntities()
    {
        // Arrange
        var financialEntities = GetTesFinancialEntities(); 

        var mockFinancialEntitiesRepository = new Mock<IFinancialEntitiesRepository>();
        mockFinancialEntitiesRepository.Setup(repo => repo.GetAllAsync())
                                .ReturnsAsync(financialEntities);

        var handler = new GetFinancialEntitiesAllQuery(mockFinancialEntitiesRepository.Object, new MapperAdapter());
        var query = new GetFinancialEntitiesAllRequest();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.FinancialEntities.Should().HaveCount(financialEntities.Count());
    }

    private IEnumerable<FinancialEntity> GetTesFinancialEntities()
    {
        return JsonLoader.LoadJson<List<FinancialEntity>>("ResponseFiles/v1/responseGetAllFinancialEntities.json");
    }
}