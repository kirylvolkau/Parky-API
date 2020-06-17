using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User model)
        {
            var user = _repository.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] User model)
        {
            bool usernameNotUnique = _repository.IsUserUnique(model.Username);
            if (usernameNotUnique)
            {
                return BadRequest("User exists.");
            }

            var user = _repository.Register(model.Username, model.Password);
            
            if (user is null)
            {
                return BadRequest(new {message = "Error while registering."});
            }

            return Ok();
        }
    }
}