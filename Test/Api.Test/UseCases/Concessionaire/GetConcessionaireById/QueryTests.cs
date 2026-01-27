using FluentAssertions;
using Moq;
using Domain.Contracts.Persistence;
using Domain.Entities;
using Api.Test.Common;
using Adapters.Mapper;
using Application.UseCases.Concessionaire.GetConcessionaireById;
using Application.Enums;

namespace Api.Test.UseCases.Concessionaire.GetConcessionaireById;

public class QueryTests
{
    [Fact]
    public async Task ValidQuery_ReturnsConcessionaire()
    {
        var company = Enums.Company.Autogermana;
        // Arrange
        var cities = GetTestConcessionaire();  /*Simulación de datos Json a Entity.*/

        var mockConcessionaireRepository = new Mock<IConcessionaireRepository>();
        mockConcessionaireRepository.Setup(repo => repo.GetAll());

        var handler = new GetConcessionaireByIdQuery(mockConcessionaireRepository.Object, new MapperAdapter());

        var query = new GetConcessionaireByIdRequest { company = company };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
      
    }

    private ConcessionaireEntity GetTestConcessionaire()
    {
        return JsonLoader.LoadJson<ConcessionaireEntity>("ResponseFiles/v1/responseGetConcessionaireById.json");
    }
}