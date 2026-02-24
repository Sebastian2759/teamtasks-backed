using Application.Base;
using Application.Dtos;
using Domain.Contracts.Adapter.Mapper;
using Domain.Contracts.Persistence;
using Domain.Entities;
using MediatR;
using System.Net;

namespace Application.UseCases.User.CreateUser;

public sealed class CreateUserCommand(
    IUsersRepository usersRepository,
    IMapperAdapter mapperAdapter
) : IRequestHandler<CreateUserRequest, ResponseBase<CreateUserResponse>>
{
    private readonly IUsersRepository _usersRepository = usersRepository;
    private readonly IMapperAdapter _mapperAdapter = mapperAdapter;

    public async Task<ResponseBase<CreateUserResponse>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseBase<CreateUserResponse>();

        var email = request.Email.Trim();

        var emailExists = await _usersRepository.ExistsAsync(x => x.Email == email, cancellationToken);
        if (emailExists)
        {
            response.StatusCode = HttpStatusCode.Conflict;
            response.Message = "El email ya existe.";
            response.Data = default!;
            return response;
        }

        var now = DateTime.UtcNow;

        var entity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = email,
            IsActive = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        _usersRepository.Add(entity);

        var dto = _mapperAdapter.Map<UserEntity, UserCreatedDto>(entity);

        response.StatusCode = HttpStatusCode.Created;
        response.Message = "Usuario creado.";
        response.Data = new CreateUserResponse { User = dto };

        return response;
    }
}