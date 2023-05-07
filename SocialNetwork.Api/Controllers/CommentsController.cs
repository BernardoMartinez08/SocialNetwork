using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SocialNetwork.Core.Entities;
using SocialNetwork.Core.Interfaces;

namespace SocialNetwork.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IRepository<Post> postRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Comment> commentRepository;
        public CommentsController(IRepository<Post> postRepository, IRepository<User> userRepository, IRepository<Comment> commentRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
            this.commentRepository = commentRepository;
        }


        [HttpGet("users/{userId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetComments([FromRoute] int userId)
        {
            var user = userRepository.GetById(userId);
            if (user == null)
                return NotFound($"No se encontro un usuario con el id {userId} para crear e post");

            var comments = commentRepository.Filter(x => x.UserId == userId);
            if (!comments.Any())
                return NotFound($"No existe ningun comentario para el usuario: {userId}");
            return Ok(comments);
            
        }

        [HttpGet("post/{postId}/[controller]/{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCommentsById([FromRoute] int postId, [FromRoute] int commentId)
        {
            var post = postRepository.GetById(postId);
            if (post == null)
                return NotFound($"No se encontro un post con el id {postId}");

            var comments = commentRepository.Filter(x => x.PostId == postId && x.Id == commentId);
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
            var post = postRepository.GetById(postId);
            if (post == null)
                return NotFound($"No se encontro un post con id: {postId} para agregar el comentario");

            var user = userRepository.GetById(comment.UserId);
            if (user == null)
                return NotFound($"No se encontro el usuario con id: {comment.UserId} para agregar el commentario");

            if (string.IsNullOrEmpty(comment.Content))
                return BadRequest("No se puede crear un comentario sin contenido");

            commentRepository.Add(comment);
            return new CreatedAtActionResult(nameof(GetCommentsById), "Comments", new { postid = postId, commentId = comment.Id }, comment);
        }

    }
}
