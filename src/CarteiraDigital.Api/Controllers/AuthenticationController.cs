using CarteiraDigital.Application.Common.Interfaces;
using CarteiraDigital.Application.Features.DTOs.Authentication;
using CarteiraDigital.Application.Features.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto dto)
        {
            var user = await _authenticationService.RegisterAsync(
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.Password);

            return CreatedAtAction(nameof(Register), new { userId = user.Id });
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
        {
            var token = await _authenticationService.LoginAsync(dto.Email, dto.Password);
            return Ok(token);
        }
    }
}
