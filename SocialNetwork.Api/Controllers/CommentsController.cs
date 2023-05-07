using System;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core;
using SocialNetwork.Core.Entities;
using SocialNetwork.Core.Interfaces;

namespace SocialNetwork.Api.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommendService commentService;

        public CommentsController(
            ICommendService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost("posts/{postId}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult CreateComment([FromRoute] int postId, [FromBody] Comment comment)
        {
            var result = this.commentService.Create(comment);
            if (result.Succeeded)
            {
                return new CreatedAtActionResult(nameof(GetCommentById), "Comments", new { postId = postId, commentId = comment.Id }, comment);
            }
            return GetErrorResult<Comment>(result);
        }

        [HttpGet("posts/{postId}/comments/{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetCommentById([FromRoute] int postId, int commentId)
        {
            var result = this.commentService.GetById(commentId, postId);
            if (result.Succeeded)
            {
                return Ok(result.Result);
            }
            return GetErrorResult<Comment>(result);
        }

        private ActionResult GetErrorResult<TResult>(OperationResult<TResult> result)
        {
            switch (result.Error.Code)
            {
                case Core.ErrorCode.NotFound:
                    return NotFound(result.Error.Message);
                case Core.ErrorCode.Unauthorized:
                    return Unauthorized(result.Error.Message);
                default:
                    return BadRequest(result.Error.Message);
            }
        }
    }
}
