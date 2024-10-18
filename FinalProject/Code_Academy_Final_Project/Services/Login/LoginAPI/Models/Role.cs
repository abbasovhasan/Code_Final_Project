namespace LoginAPI.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Kullanıcılar ile ilişki
    public ICollection<User> Users { get; set; }
}


