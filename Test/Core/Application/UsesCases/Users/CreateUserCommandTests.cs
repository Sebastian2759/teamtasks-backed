using Application.Dtos;
using Application.UseCases.User.CreateUser;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using System.Net;

namespace Test.Core.Application.UsesCases.Users;

public class CreateUserCommandTests
{
    private readonly Mock<IUsersRepository> _usersRepo = new();
    private readonly Mock<IMapperAdapter> _mapper = new();

    private CreateUserCommand CreateSut()
        => new(_usersRepo.Object, _mapper.Object);

    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ReturnsConflict_AndDoesNotAdd()
    {
        // Arrange
        var sut = CreateSut();

        var req = new CreateUserRequest
        {
            Name = " Ana  ",
            Email = " ana@empresa.com "
        };

        _usersRepo
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<UserEntity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var res = await sut.Handle(req, CancellationToken.None);

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.Conflict);
        res.Message.Should().Be("El email ya existe.");
        res.Data.Should().BeNull();

        _usersRepo.Verify(r => r.Add(It.IsAny<UserEntity>()), Times.Never);
        _mapper.Verify(m => m.Map<UserEntity, UserCreatedDto>(It.IsAny<UserEntity>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValid_CreatesUser_ReturnsCreated_AndMapsDto()
    {
        // Arrange
        var sut = CreateSut();

        var req = new CreateUserRequest
        {
            Name = "  Ana Perez  ",
            Email = "  ana@empresa.com  "
        };

        Expression<Func<UserEntity, bool>>? capturedPredicate = null;

        _usersRepo
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<UserEntity, bool>>>(), It.IsAny<CancellationToken>()))
            .Callback<Expression<Func<UserEntity, bool>>, CancellationToken>((pred, _) => capturedPredicate = pred)
            .ReturnsAsync(false);

        UserEntity? capturedEntity = null;

        _usersRepo
            .Setup(r => r.Add(It.IsAny<UserEntity>()))
            .Callback<UserEntity>(e => capturedEntity = e);

        _mapper
            .Setup(m => m.Map<UserEntity, UserCreatedDto>(It.IsAny<UserEntity>()))
            .Returns((UserEntity e) => new UserCreatedDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email
            });

        var before = DateTime.UtcNow;

        // Act
        var res = await sut.Handle(req, CancellationToken.None);

        var after = DateTime.UtcNow;

        // Assert - Response
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        res.Message.Should().Be("Usuario creado.");
        res.Data.Should().NotBeNull();
        res.Data.User.Should().NotBeNull();
        res.Data.User.Email.Should().Be("ana@empresa.com");
        res.Data.User.Name.Should().Be("Ana Perez");

        // Assert - Repository calls
        _usersRepo.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<UserEntity, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _usersRepo.Verify(r => r.Add(It.IsAny<UserEntity>()), Times.Once);
        _mapper.Verify(m => m.Map<UserEntity, UserCreatedDto>(It.IsAny<UserEntity>()), Times.Once);

        // Assert - Predicate is using trimmed email
        capturedPredicate.Should().NotBeNull();
        var fn = capturedPredicate!.Compile();
        fn(new UserEntity { Email = "ana@empresa.com" }).Should().BeTrue();
        fn(new UserEntity { Email = "otro@empresa.com" }).Should().BeFalse();

        // Assert - Entity built correctly
        capturedEntity.Should().NotBeNull();
        capturedEntity!.Id.Should().NotBe(Guid.Empty);
        capturedEntity.Name.Should().Be("Ana Perez");
        capturedEntity.Email.Should().Be("ana@empresa.com");
        capturedEntity.IsActive.Should().BeTrue();

        // Timestamps within execution window
        capturedEntity.CreatedAtUtc.Should().BeOnOrAfter(before).And.BeOnOrBefore(after.AddSeconds(2));
        capturedEntity.UpdatedAtUtc.Should().BeOnOrAfter(before).And.BeOnOrBefore(after.AddSeconds(2));

        // DTO corresponds to entity
        res.Data.User.Id.Should().Be(capturedEntity.Id);
    }
}