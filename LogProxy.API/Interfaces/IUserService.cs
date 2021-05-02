using LogProxy.API.Entities;
using System.Threading.Tasks;

namespace LogProxy.API.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }
}