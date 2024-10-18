using LoginAPI.DTOs;

namespace LoginAPI.Services;

public interface IUserService
{
    Task<string> RegisterAsync(RegisterDto registerDto);
    Task<string> LoginAsync(LoginDto loginDto);
}