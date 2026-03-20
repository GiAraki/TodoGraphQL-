using System.Security.Claims;

namespace TodoGraphQL.Services;

public class UserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Lê o userId que está dentro do token JWT
    public int GetUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (claim == null)
            throw new GraphQLException("Usuário não autenticado.");

        return int.Parse(claim);
    }
}