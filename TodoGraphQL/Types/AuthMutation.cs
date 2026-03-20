using TodoGraphQL.Models;
using TodoGraphQL.Services;

namespace TodoGraphQL.Types;

[ExtendObjectType("Mutation")]
public class AuthMutation
{
    public async Task<AuthPayload> Register(
        string email,
        string password,
        [Service] UserRepository userRepo,
        [Service] TokenService tokenService)
    {
        if (await userRepo.FindByEmail(email) != null)
            throw new GraphQLException("Email já cadastrado.");

        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = await userRepo.Create(email, hash);
        var token = tokenService.GenerateToken(user);

        return new AuthPayload(token, user.Email);
    }

    public async Task<AuthPayload> Login(
        string email,
        string password,
        [Service] UserRepository userRepo,
        [Service] TokenService tokenService)
    {
        var user = await userRepo.FindByEmail(email)
            ?? throw new GraphQLException("Usuário não encontrado.");

        var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!valid)
            throw new GraphQLException("Senha incorreta.");

        var token = tokenService.GenerateToken(user);
        return new AuthPayload(token, user.Email);
    }
}

public record AuthPayload(string Token, string Email);