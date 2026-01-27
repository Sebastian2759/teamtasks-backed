using FluentAssertions;
using Moq;
using Domain.Contracts.Persistence;
using Domain.Entities;
using Api.Test.Common;
using Adapters.Mapper;
using Application.UseCases.City.GetCityByDepartmentId;

namespace Api.Test.UseCases.City.GetCityByDepartmentId;

public class QueryTests
{
    [Fact]
    public async Task ValidQuery_ReturnsDepartments()
    {
        // Arrange
        var cities = GetTestCities(); // Obtén las ciudades de prueba

        var mockCityRepository = new Mock<ICityRepository>();
        mockCityRepository.Setup(repo => repo.GetByDepartmentIdAsync("25"))
                          .ReturnsAsync(cities);

        var handler = new GetCityByDepartmentIdQuery(mockCityRepository.Object, new MapperAdapter());
        var query = new GetCityByDepartmentIdRequest { DepartmentId = "25" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Cities.Should().HaveCount(cities.Count());
    }

    private IEnumerable<CityEntity> GetTestCities()
    {
        return JsonLoader.LoadJson<List<CityEntity>>("ResponseFiles/v1/responseGetCityByDepartmentId.json");
    }
}