using Asp.Versioning;
using DigiInv.Application.DTOs;
using DigiInv.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigiInv.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var response = await _authService.LoginAsync(loginDto);
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var response = await _authService.RegisterAsync(registerDto);
        return Ok(response);
    }
}
