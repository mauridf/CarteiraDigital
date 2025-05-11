using CarteiraDigital.Application.Features.DTOs.Authentication;
using CarteiraDigital.Application.Features.DTOs.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

public class AuthenticationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthenticationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsCreated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var dto = new CreateUserDto(
            "Test",
            "User",
            $"test_{Guid.NewGuid()}@example.com",
            "Test@123");

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var client = _factory.CreateClient();
        var registerDto = new CreateUserDto(
            "Test",
            "User",
            $"test_{Guid.NewGuid()}@example.com",
            "Test@123");

        await client.PostAsJsonAsync("/api/auth/register", registerDto);

        var loginDto = new LoginDto(registerDto.Email, registerDto.Password);

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var token = await response.Content.ReadFromJsonAsync<TokenDto>();
        Assert.NotNull(token);
        Assert.NotNull(token.Token);
        Assert.True(token.Expiration > DateTime.UtcNow);
    }
}