using LoginAPI.DTOs;
using LoginAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    // Kullanıcı Kayıt
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _userService.RegisterAsync(registerDto);
        return Ok(result);
    }

    // Kullanıcı Giriş
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var token = await _userService.LoginAsync(loginDto);
        if (token == "Invalid username or password.")
        {
            return Unauthorized(token);
        }
        return Ok(new { Token = token });
    }
}
