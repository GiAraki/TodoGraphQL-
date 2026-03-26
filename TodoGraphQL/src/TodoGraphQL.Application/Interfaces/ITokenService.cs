using TodoGraphQL.Domain.Entities;

namespace TodoGraphQL.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}