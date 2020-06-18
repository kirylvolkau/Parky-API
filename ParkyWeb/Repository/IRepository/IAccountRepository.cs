using System.Threading.Tasks;
using ParkyWeb.Models;

namespace ParkyWeb.Repository.IRepository
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User user);
        Task<bool> RegisterAsync(string url, User user);
    }
}