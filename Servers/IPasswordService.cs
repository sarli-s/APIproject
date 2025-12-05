using Entitys;

namespace Servers
{
    public interface IPasswordService
    {
        Password CheckPassword(string password);
    }
}