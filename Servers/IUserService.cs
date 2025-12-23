using DTOs;
using Entitys;
using Repository;

namespace Servers
{
    public interface IUserService
    {
        Task<UserDTO> AddUser(UserDTO user,string password);
        void DeleteUser(int id);
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> Login(LoginUserDTO loginUser);
        Task<bool> UpdateUser(int id, UserDTO user, string password);
    }
}