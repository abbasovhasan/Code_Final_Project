using LoginAPI.DTOs;
using LoginAPI.Repositories;
using System.Security.Cryptography;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(registerDto.Username);
        if (existingUser != null)
        {
            return "Username already exists.";
        }

        var passwordHash = HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            PasswordHash = passwordHash,
            Email = registerDto.Email,
            RoleId = 2 // Varsayılan olarak "User" rolü atanıyor
        };

        await _userRepository.AddAsync(user);

        return "User registered successfully!";
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return "Invalid username or password.";
        }

        var token = _tokenService.GenerateToken(user.Username, user.Role.Name);
        return token;
    }

    private string HashPassword(string password)
    {
        using var sha256 =  SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        var hashedPassword = HashPassword(password);
        return hashedPassword == passwordHash;
    }
}
