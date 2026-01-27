using FluentAssertions;
using Moq;
using Domain.Entities;
using Api.Test.Common;
using Adapters.Mapper;
using Application.UseCases.FinancialAdvisors.FinancialAdvisorsAll;
using Persistence.Repositories;


namespace Api.Test.UseCases.FinancialAdvisors.FinancialAdvisorsAll;

public class QueryTests
{
    [Fact]
    public async Task ValidQuery_ReturnsFinancialAdvisors()
    {
        // Arrange
        var financialAdvisors = GetTestFinancialAdvisors(); 

        var mockFinancialAdvisorsRepository = new Mock<IFinancialAdvisorsRepository>();
        mockFinancialAdvisorsRepository.Setup(repo => repo.GetFinancialAdvisorsAllAsync())
                                .ReturnsAsync(financialAdvisors);

        var handler = new GetFinancialAdvisorsAllQuery(mockFinancialAdvisorsRepository.Object, new MapperAdapter());
        var query = new GetFinancialAdvisorsAllRequest();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

    }

    private IEnumerable<FinancialAdvisorsEntity> GetTestFinancialAdvisors()
    {
        return JsonLoader.LoadJson<List<FinancialAdvisorsEntity>>("ResponseFiles/v1/responseGetFinancialAdvisors.json");
    }
}