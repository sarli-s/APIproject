using System.Text.Json;
using Entitys;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        dbSHOPContext _dbSHOPContext;
        public UserRepository(dbSHOPContext dbSHOPContext)
        {
            _dbSHOPContext = dbSHOPContext;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _dbSHOPContext.FindAsync<User>(id);
        }


        public async Task<User> AddUser(User user)
        {
            await _dbSHOPContext.AddAsync(user);
            await _dbSHOPContext.SaveChangesAsync();
            return await _dbSHOPContext.FindAsync<User>(user.UserId);
        }



        public async Task<User> Login(LoginUser loginUser)
        {

            User? user = await _dbSHOPContext.Users.FirstOrDefaultAsync(x=>x.UserEmail==loginUser.LoginUserEmail &&
            x.UserPassword==loginUser.LoginUserPassword);

            return user;
        }


        public async Task UpdateUser(int id, User updatedUser)
        {
            _dbSHOPContext.Users.Update(updatedUser);
            await _dbSHOPContext.SaveChangesAsync();

        }


        public void DeleteUser(int id)
        {
        }
    }
}
