using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly AppSettings _settings;

        public UserRepository(AppDbContext context, IOptions<AppSettings> settings)
        {
            _context = context;
            _settings = settings.Value;
        }
        
        public bool IsUserUnique(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public User Authenticate(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                return null;
            }
            //generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = string.Empty;
            return user;
        }

        public User Register(string username, string password)
        {
            var user = new User
            {
                Username = username, 
                Password = password,
                Role = "Admin"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            user.Password = string.Empty;
            return user;
        }
    }
}