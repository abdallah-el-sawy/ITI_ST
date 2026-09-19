using Lab3.Models;

namespace Lab3.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUserNameAsync(string userName);
        Task<bool> IsDuplicateUserNameAsync(string userName);
    }
}