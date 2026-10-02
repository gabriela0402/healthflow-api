using HealthFlow.Model.DTOs.Auth;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HealthFlow.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Jwt:Key"] =
                        "HealthFlow-Test-Key-2026-"
                        + "Long-Secret-For-Tests"
                })
            .Build();

        _service = new AuthService(
            _userRepositoryMock.Object,
            configuration);
    }

    [Fact]
    public async Task Deve_cadastrar_usuario()
    {
        _userRepositoryMock
            .Setup(repository => repository.GetByEmailAsync(
                "gabriela@email.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var request = new RegisterRequest(
            "Gabriela",
            "gabriela@email.com",
            "123456");

        var result = await _service.RegisterAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Gabriela", result.Name);
        Assert.Equal("gabriela@email.com", result.Email);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));

        _userRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<User>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _userRepositoryMock.Verify(
            repository => repository.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Nao_deve_cadastrar_email_duplicado()
    {
        var existingUser = new User(
            "Gabriela",
            "gabriela@email.com",
            "hash");

        _userRepositoryMock
            .Setup(repository => repository.GetByEmailAsync(
                "gabriela@email.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var request = new RegisterRequest(
            "Outra pessoa",
            "gabriela@email.com",
            "123456");

        var exception = await Assert.ThrowsAsync<
            BusinessRuleException>(
            () => _service.RegisterAsync(request));

        Assert.Equal(
            "Já existe um usuário com este e-mail.",
            exception.Message);

        _userRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<User>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Deve_fazer_login_com_senha_correta()
    {
        var user = new User(
            "Gabriela",
            "gabriela@email.com",
            string.Empty);

        var passwordHasher =
            new Microsoft.AspNetCore.Identity.PasswordHasher<User>();

        user.SetPasswordHash(
            passwordHasher.HashPassword(user, "123456"));

        _userRepositoryMock
            .Setup(repository => repository.GetByEmailAsync(
                "gabriela@email.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new LoginRequest(
            "gabriela@email.com",
            "123456");

        var result = await _service.LoginAsync(request);

        Assert.NotNull(result);
        Assert.Equal("gabriela@email.com", result.Email);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task Nao_deve_fazer_login_com_senha_incorreta()
    {
        var user = new User(
            "Gabriela",
            "gabriela@email.com",
            string.Empty);

        var passwordHasher =
            new Microsoft.AspNetCore.Identity.PasswordHasher<User>();

        user.SetPasswordHash(
            passwordHasher.HashPassword(user, "123456"));

        _userRepositoryMock
            .Setup(repository => repository.GetByEmailAsync(
                "gabriela@email.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new LoginRequest(
            "gabriela@email.com",
            "senha-incorreta");

        var exception = await Assert.ThrowsAsync<
            BusinessRuleException>(
            () => _service.LoginAsync(request));

        Assert.Equal(
            "E-mail ou senha inválidos.",
            exception.Message);
    }
}
