using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Application.Interfaces;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Auth;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RegisterUseCase(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthDto> ExecuteAsync(string email, string password)
    {
        var existing = await _userRepository.FindByEmailAsync(email);
        if (existing != null)
            throw new DomainException("Email já cadastrado.");

        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = User.Create(email, hash, UserRole.User);
        var saved = await _userRepository.CreateAsync(user);
        var token = _tokenService.GenerateToken(saved);

        return new AuthDto(token, saved.Email, saved.Role.ToString());
    }
}