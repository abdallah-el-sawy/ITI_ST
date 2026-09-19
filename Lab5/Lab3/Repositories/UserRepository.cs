using Microsoft.EntityFrameworkCore;
using Lab3.Data;
using Lab3.Models;

namespace Lab3.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }

        public async Task<bool> IsDuplicateUserNameAsync(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
        }
    }
}