using Microsoft.Extensions.Logging;
using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Application.Interfaces;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<LoginUseCase> _logger;

    public LoginUseCase(
        IUserRepository userRepository,
        ITokenService tokenService,
        ILogger<LoginUseCase> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthDto> ExecuteAsync(string email, string password)
    {
        _logger.LogInformation("Tentativa de login para {Email}", email);

        var user = await _userRepository.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("Login falhou — usuário não encontrado: {Email}", email);
            throw new DomainException("Usuário não encontrado.");
        }

        var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!valid)
        {
            _logger.LogWarning("Login falhou — senha incorreta para: {Email}", email);
            throw new DomainException("Senha incorreta.");
        }

        _logger.LogInformation("Login bem-sucedido para {Email} | Role: {Role}", email, user.Role);

        var token = _tokenService.GenerateToken(user);
        return new AuthDto(token, user.Email, user.Role.ToString());
    }
}