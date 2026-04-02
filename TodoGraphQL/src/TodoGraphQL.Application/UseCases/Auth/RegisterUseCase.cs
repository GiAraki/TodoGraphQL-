using Microsoft.Extensions.Logging;
using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Application.Interfaces;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Auth;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<RegisterUseCase> _logger;

    public RegisterUseCase(
        IUserRepository userRepository,
        ITokenService tokenService,
        ILogger<RegisterUseCase> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthDto> ExecuteAsync(string email, string password)
    {
        _logger.LogInformation("Tentativa de cadastro para {Email}", email);

        var existing = await _userRepository.FindByEmailAsync(email);
        if (existing != null)
        {
            _logger.LogWarning("Cadastro falhou — email já existe: {Email}", email);
            throw new DomainException("Email já cadastrado.");
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = User.Create(email, hash, UserRole.User);
        var saved = await _userRepository.CreateAsync(user);

        _logger.LogInformation("Usuário criado com sucesso: {Email} | Id: {UserId}", email, saved.Id);

        var token = _tokenService.GenerateToken(saved);
        return new AuthDto(token, saved.Email, saved.Role.ToString());
    }
}