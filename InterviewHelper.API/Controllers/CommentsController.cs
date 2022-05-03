using System.Net;
using System.Security.Claims;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CommentsController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly ICommentService _commentService;

        public CommentsController(UserRepository userRepository, ICommentService commentService)
        {
            _userRepository = userRepository;
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult GetQuestionComments()
        {
            throw new NotImplementedException();
        }

        [HttpPost("add")]
        public IActionResult AddComment(CommentRequest newComment)
        {
            try
            {
                var response = _commentService.AddComment(newComment);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        public IActionResult EditComment(Comment comment)
        {
            var sessionUserEmail = User.FindFirst(ClaimTypes.Email).Value;
            try
            {
                var commentOwner = _commentService.GetCommentOwnerById(comment.Id);
                if (sessionUserEmail != commentOwner.Email)
                    return BadRequest("User is not authorized to perform this action");
            }
            catch (UserNotFoundException)
            {
                return BadRequest("Comment owner not found");
            }

            try
            {
                _commentService.EditComment(comment);
                return Ok();
            }
            catch (CommentNotFoundException)
            {
                return BadRequest(new {message = "Comment not found"});
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{commentId:int}")]
        public IActionResult DeleteComment(int commentId)
        {
            var sessionUserEmail = User.FindFirst(ClaimTypes.Email).Value;
            try
            {
                var commentOwner = _commentService.GetCommentOwnerById(commentId);
                if (sessionUserEmail != commentOwner.Email)
                    return BadRequest("User is not authorized to perform this action");
                _commentService.DeleteComment(commentId);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return BadRequest("Comment owner not found");
            }
            catch (CommentNotFoundException)
            {
                return BadRequest("Comment not found");
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}