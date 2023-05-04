using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Api.Models;

namespace SocialNetwork.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private SocialNetworkContext context;

        public PostsController(SocialNetworkContext context) { 
            this.context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">El id del usuario del que se quieren extraer sus posts.</param>
        /// <returns>Los post para el usuario enviado.</returns>
        [HttpGet("users/{userId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetPost([FromRoute] int userId)
        {
            if (!context.Users.Any())
                return NotFound("No hay usuarios");

            var user = context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return NotFound($"No se encontro un usuario con el id {userId}");

            var posts = context.Posts.Where(x => x.UserId == userId);
            if (!posts.Any())
                return NotFound($"No existe ningun post para el usuario: {userId}");
            return Ok(posts);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">El id del usuario del que se quieren extraer sus posts.</param>
        /// <returns>Los post para el usuario enviado.</returns>
        [HttpGet("users/{userId}/[controller]/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetPostById([FromRoute] int userId, [FromRoute] int postId)
        {
            if (!context.Users.Any())
                return NotFound("No hay usuarios");

            var user = context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return NotFound($"No se encontro un usuario con el id {userId}");

            var posts = context.Posts.Where(x => x.UserId == userId && x.Id == postId);
            if (!posts.Any())
                return NotFound($"No existe ningun post para el usuario: {userId}");
            return Ok(posts);
        }


        /// <summary>
        /// Agrega una publicacion para el usuario.
        /// </summary>
        /// <param name="userId">El id del usuario que agregara la publicacion.</param>
        /// <param name="post">La publicacion a agregar.</param>
        /// <returns>La publicacion agregada.</returns>
        [HttpPost("users/{userId}/Posts")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddPost([FromRoute] int userId, [FromBody] Post post)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return BadRequest($"No se encontro un usuario con el id {userId} para crear e post");

            if (string.IsNullOrEmpty(post.Content))
                return BadRequest("No se puede crear un post sin contenido");

            context.Posts.Add(post);
            context.SaveChanges();
            return new CreatedAtActionResult(nameof(GetPostById), "Posts", new { userId = userId, postId = post.Id }, post);
        }
    }
}
