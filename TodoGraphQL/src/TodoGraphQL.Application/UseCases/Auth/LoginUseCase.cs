using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Application.Interfaces;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginUseCase(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthDto> ExecuteAsync(string email, string password)
    {
        var user = await _userRepository.FindByEmailAsync(email)
            ?? throw new DomainException("Usuário não encontrado.");

        var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!valid)
            throw new DomainException("Senha incorreta.");

        var token = _tokenService.GenerateToken(user);
        return new AuthDto(token, user.Email);
    }
}