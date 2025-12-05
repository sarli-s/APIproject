using Entitys;
using Repository;

namespace Servers
{
    public interface IUserService
    {
        Task<User> AddUser(User user);
        void DeleteUser(int id);
        Task<User> GetUserById(int id);
        Task<User> Login(LoginUser loginUser);
        Task<bool> UpdateUser(int id, User user);
    }
}