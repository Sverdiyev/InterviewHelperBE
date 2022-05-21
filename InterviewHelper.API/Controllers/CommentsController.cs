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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public CommentsController(ICommentService commentService, IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        [HttpGet("{questionId:int}")]
        public IActionResult GetQuestionComments(int questionId)
        {
            try
            {
                return Ok(_commentService.GetAllQuestionComments(questionId));
            }
            catch (QuestionNotFoundException)
            {
                return BadRequest("Question not found");
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("add")]
        public IActionResult AddComment(CommentRequest newComment)
        {
            var user = GetLoggedInUser();
            try
            {
                var response = _commentService.AddComment(newComment, user.Id);
                return Ok(response);
            }
            catch (UserNotFoundException)
            {
                return BadRequest("User not found");
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        public IActionResult EditComment(Comment comment)
        {
            try
            {
                var sessionUserEmail = User.FindFirst(ClaimTypes.Email).Value;
                if (!_commentService.CommentBelongsToUser(sessionUserEmail, comment.Id))
                {
                    return BadRequest("User is not allowed to modify the comment");
                }

                _commentService.EditComment(comment);
                return Ok();
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

        [HttpDelete("{commentId:int}")]
        public IActionResult DeleteComment(int commentId)
        {
            try
            {
                var sessionUserEmail = User.FindFirst(ClaimTypes.Email).Value;
                if (!_commentService.CommentBelongsToUser(sessionUserEmail, commentId))
                {
                    return BadRequest("User is not allowed to delete the comment");
                }

                _commentService.DeleteComment(commentId);
                return Ok();
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
        private User GetLoggedInUser()
        {
            return _userService.GetUserByEmail(User.FindFirst(ClaimTypes.Email).Value);
        }
    }
}