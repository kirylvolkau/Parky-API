using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public bool IsUserUnique(string username)
        {
            throw new System.NotImplementedException();
        }

        public User Authenticate(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public User Register(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}