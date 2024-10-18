using LoginAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }

    // Role ile ilişki
    public int RoleId { get; set; } // Foreign key olarak RoleId
    public Role Role { get; set; } // Role nesnesi
}

