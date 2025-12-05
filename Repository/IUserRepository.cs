using Entitys;

namespace Repository
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);
        void DeleteUser(int id);
        Task<User> GetUserById(int id);
        Task<User> Login(LoginUser loginUser);
        void UpdateUser(int id, User updatedUser);
    }
}