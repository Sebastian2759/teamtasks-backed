using FluentAssertions;
using Moq;
using Domain.Contracts.Persistence;
using Domain.Entities;
using Api.Test.Common;
using Adapters.Mapper;
using Application.UseCases.Department.GetDepartmentAll;

namespace Api.Test.UseCases.Department.GetDepartmentAll;

public class QueryTests
{
    [Fact]
    public async Task ValidQuery_ReturnsDepartments()
    {
        // Arrange
        var departments = GetTestDepartments(); // Obtén los departamentos de prueba

        var mockDepartmentRepository = new Mock<IDepartmentRepository>();
        mockDepartmentRepository.Setup(repo => repo.GetAllAsync())
                                .ReturnsAsync(departments);

        var handler = new GetDepartmentAllQuery(mockDepartmentRepository.Object, new MapperAdapter());
        var query = new GetDepartmentAllRequest();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Departments.Should().HaveCount(departments.Count());
    }

    private IEnumerable<DepartmentEntity> GetTestDepartments()
    {
        return JsonLoader.LoadJson<List<DepartmentEntity>>("ResponseFiles/v1/responseGetDepartmentColombia.json");
    }
}