namespace LoginAPI.DTOs;

public class RegisterDto
{
    public string Username { get; set; }  // Kullanıcı adı
    public string Password { get; set; }  // Şifre
    public string Email { get; set; }  // E-posta adresi
}
