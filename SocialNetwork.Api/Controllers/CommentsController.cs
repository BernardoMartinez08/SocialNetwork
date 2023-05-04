using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Api.Models;
using System.Diagnostics;

namespace SocialNetwork.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private SocialNetworkContext context;

        public CommentsController(SocialNetworkContext context)
        {
            this.context = context;
        }


        [HttpGet("users/{userId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetComments([FromRoute] int userId)
        {
            if (!context.Users.Any())
                return NotFound("No hay usuarios");

            var user = context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return NotFound($"No se encontro un usuario con el id {userId} para crear e post");

            var comments = context.Comments.Where(x => x.UserId == userId);
            if (!comments.Any())
                return NotFound($"No existe ningun comentario para el usuario: {userId}");
            return Ok(comments);
            
        }

        [HttpGet("post/{postId}/[controller]/{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCommentsById([FromRoute] int postId, [FromRoute] int commentId)
        {
            if (!context.Posts.Any())
                return NotFound("No hay post");

            var post = context.Posts.FirstOrDefault(x => x.Id == postId);
            if (post == null)
                return NotFound($"No se encontro un post con el id {postId}");

            var comments = context.Comments.Where(x => x.PostId == postId && x.Id == commentId);
            if (!comments.Any())
                return NotFound($"No existe ningun comentario para el post: {postId}");
            return Ok(comments);
            
        }


        [HttpPost("posts/{postId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult CreateComment([FromRoute] int postId, [FromBody] Comment comment)
        {
            var post = context.Posts.FirstOrDefault(x => x.Id == postId);
            if (post == null)
                return NotFound($"No se encontro un post con id: {postId} para agregar el comentario");

            var user = context.Users.FirstOrDefault(x => x.Id == postId);
            if (user == null)
                return NotFound($"No se encontro el usuario con id: {comment.UserId} para agregar el commentario");

            if (string.IsNullOrEmpty(comment.Content))
                return BadRequest("No se puede crear un comentario sin contenido");

            context.Comments.Add(comment);
            context.SaveChanges();
            return new CreatedAtActionResult(nameof(GetCommentsById), "Comments", new { postid = postId, commentId = comment.Id }, comment);
        }

    }
}
