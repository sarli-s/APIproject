using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);
        Task<User> GetUserById(int id);
        Task<User> Login(string email);
        Task UpdateUser(User updatedUser);
        Task DeleteUser(int id);
        Task<User> GetUserByEmail(string email);
    }
}