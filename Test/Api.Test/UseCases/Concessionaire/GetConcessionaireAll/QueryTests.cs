using Adapters.Mapper;
using Api.Test.Common;
using Application.UseCases.Concessionaire.GetConcessionaireAll;
using Domain.Contracts.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Api.Test.UseCases.Concessionaire.GetConcessionaireAll;

public class QueryTests
{
    [Fact]
    public async Task ValidQuery_ReturnsConcessionaires()
    {
        // Arrange
        IEnumerable<ConcessionaireEntity> concessionaires = GetTestConcessionaires();

        var mockConcessionaireRepository = new Mock<IConcessionaireRepository>();
        mockConcessionaireRepository.Setup(repo => repo.GetAllAsync())
                                .ReturnsAsync(concessionaires);

        var handler = new GetConcessionaireAllQuery(mockConcessionaireRepository.Object, new MapperAdapter());
        var query = new GetConcessionaireAllRequest();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Concessionaires.Should().HaveCount(concessionaires.Count());
    }

    private IEnumerable<ConcessionaireEntity> GetTestConcessionaires()
    {
        return JsonLoader.LoadJson<List<ConcessionaireEntity>>("ResponseFiles/v1/responseGetAllConcessionaires.json");
    }
}
