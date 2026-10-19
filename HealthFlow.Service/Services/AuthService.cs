using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthFlow.Model.DTOs.Auth;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HealthFlow.Service.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        IPatientRepository patientRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _patientRepository = patientRepository;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<AuthResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new BusinessRuleException(
                "O nome é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new BusinessRuleException(
                "O e-mail é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(request.Password) ||
            request.Password.Length < 6)
        {
            throw new BusinessRuleException(
                "A senha deve possuir pelo menos 6 caracteres.");
        }

        var name = request.Name.Trim();
        var email = request.Email.Trim().ToLowerInvariant();

        var existingUser =
            await _userRepository.GetByEmailAsync(
                email,
                cancellationToken);

        if (existingUser is not null)
        {
            throw new BusinessRuleException(
                "Já existe um usuário com este e-mail.");
        }

        var existingPatient =
            await _patientRepository.GetByEmailAsync(
                email,
                cancellationToken);

        if (existingPatient is not null)
        {
            throw new BusinessRuleException(
                "Já existe um paciente com este e-mail.");
        }

        var user = new User(
            name,
            email,
            string.Empty,
            "Patient");

        user.SetPasswordHash(
            _passwordHasher.HashPassword(
                user,
                request.Password));

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        await _userRepository.SaveChangesAsync(
            cancellationToken);

        var patient = new Patient(
            name,
            email,
            string.Empty,
            new DateTime(1900, 1, 1));


        await _patientRepository.AddAsync(
            patient,
            cancellationToken);

        await _patientRepository.SaveChangesAsync(
            cancellationToken);

        return CreateAuthResponse(user, patient);
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user =
            await _userRepository.GetByEmailAsync(
                email,
                cancellationToken);

        if (user is null)
        {
            throw new BusinessRuleException(
                "E-mail ou senha inválidos.");
        }

        var passwordResult =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

        if (passwordResult ==
            PasswordVerificationResult.Failed)
        {
            throw new BusinessRuleException(
                "E-mail ou senha inválidos.");
        }

        var patient =
            await _patientRepository.GetByEmailAsync(
                user.Email,
                cancellationToken);

        return CreateAuthResponse(user, patient);
    }

    private AuthResponse CreateAuthResponse(
        User user,
        Patient? patient)
    {
        var token = GenerateToken(user);

        return new AuthResponse(
            user.Id,
            patient?.Id,
            user.Name,
            user.Email,
            user.Role,
            token);
    }

    private string GenerateToken(User user)
    {
        var key = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(
                "A chave JWT não foi configurada.");
        }

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Name,
                user.Name),

            new Claim(
                ClaimTypes.Email,
                user.Email),

            new Claim(
                ClaimTypes.Role,
                user.Role)
        };

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
