using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Api.Models;
using System.Xml.Linq;


namespace SocialNetwork.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private SocialNetworkContext context;

        public UserController(SocialNetworkContext context)
        {
            this.context = context;
        }

        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromQuery] string ? username)
        {
            if (!context.Users.Any())
                return NotFound("No hay usuarios");

            if (string.IsNullOrEmpty(username))
                return Ok(context.Users);

            var user = context.Users.Where(x => x.Username.StartsWith(username));
            if (!user.Any())
                return NotFound($"No existe ningun usuario con el username: {username}");
            return Ok(user);
            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var user = context.Users.First(x => x.Id == id);
            if (user == null)
                return NotFound($"No se encontro el usuario con id: {id}");
            return Ok(user);
            
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return Ok(user);
            
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            var userToRemove = context.Users.First(x => x.Id == id);
            context.Users.Remove(userToRemove);
            context.Users.Add(user);
            context.SaveChanges();
            return Ok(user);
            
        }
    }
}
