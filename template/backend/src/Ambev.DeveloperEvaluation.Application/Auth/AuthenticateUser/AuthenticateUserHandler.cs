using Ambev.DeveloperEvaluation.Application.Common.Results;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

public sealed class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, CommandResult<AuthenticateUserResult>>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _tokens;

    public AuthenticateUserHandler(
        IUserRepository users,
        IPasswordHasher hasher,
        IJwtTokenGenerator tokens)
    {
        _users = users;
        _hasher = hasher;
        _tokens = tokens;
    }

    public async Task<CommandResult<AuthenticateUserResult>> Handle(
        AuthenticateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !_hasher.VerifyPassword(request.Password, user.Password))
            return CommandResultFactory.Unauthorized<AuthenticateUserResult>(
                "auth.invalid_credentials",
                "Invalid credentials.");

        if (!new ActiveUserSpecification().IsSatisfiedBy(user))
            return CommandResultFactory.Unauthorized<AuthenticateUserResult>(
                "auth.inactive_user",
                "User is not active.");

        return CommandResultFactory.Success(new AuthenticateUserResult
        {
            Token = _tokens.GenerateToken(user),
            Email = user.Email,
            Name = user.Username,
            Role = user.Role.ToString()
        });
    }
}
