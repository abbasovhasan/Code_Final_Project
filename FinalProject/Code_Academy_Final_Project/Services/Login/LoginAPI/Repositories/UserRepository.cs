namespace LoginAPI.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.Include(u => u.Role) // Rolü de dahil ediyoruz
                                       .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
