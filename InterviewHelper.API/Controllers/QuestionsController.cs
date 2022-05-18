using System.Net;
using System.Security.Claims;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.RequestsModels;
using InterviewHelper.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly ILogger<QuestionsController> _logger;
        private readonly IQuestionsService _questionsService;
        private readonly IUserService _userService;

        public QuestionsController(ILogger<QuestionsController> logger, IQuestionsService questionsService,
            IUserService userService)
        {
            _logger = logger;
            _questionsService = questionsService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetQuestions(string? search)
        {
            var userSessionEmail = User.FindFirst(ClaimTypes.Email).Value;
            var user = _userService.GetUserByEmail(userSessionEmail);

            var questions = _questionsService.GetQuestions(search, user.Id);

            return Ok(questions);
        }

        [HttpPost("add")]
        public IActionResult PostAddQuestion(RequestQuestion newQuestion)
        {
            try
            {
                _questionsService.AddQuestion(newQuestion);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> UpdateQuestion(RequestQuestion updatedQuestion)
        {
            try
            {
                await _questionsService.UpdateQuestion(updatedQuestion);
                return Ok();
            }
            catch (QuestionNotFoundException ex)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{questionId:int}")]
        public IActionResult DeleteQuestion(int questionId)
        {
            try
            {
                _questionsService.DeleteQuestion(questionId);
                return Ok();
            }
            catch (QuestionNotFoundException)
            {
                return BadRequest(new {message = "Question not found"});
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("votes/upvote")]
        public IActionResult UpvoteQuestion([FromBody] int questionId)
        {
            var user = GetLoggedInUser();
            try
            {
                _questionsService.UpVoteQuestion(questionId, user);
                return Ok();
            }
            catch (QuestionNotFoundException)
            {
                return BadRequest(new {message = "Question not found"});
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("votes/downvote")]
        public IActionResult DownvoteQuestion([FromBody] int questionId)
        {
            var user = GetLoggedInUser();
            try
            {
                _questionsService.DownVoteQuestion(questionId, user);
                return Ok();
            }
            catch (QuestionNotFoundException)
            {
                return BadRequest(new {message = "Question not found"});
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("votes")]
        public IActionResult DeleteQuestionVote([FromBody] int questionId)
        {
            var user = GetLoggedInUser();
            try
            {
                _questionsService.DeleteUserVote(questionId, user);
                return Ok();
            }
            catch (QuestionNotFoundException)
            {
                return BadRequest(new {message = "Question not found"});
            }
            catch (VoteNotFoundException)
            {
                return BadRequest(new {message = "Vote not found"});
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("favourites/add")]
        public IActionResult AddFavouriteQuestion([FromBody] int questionId)
        {
            var user = GetLoggedInUser();
            try
            {
                _questionsService.AddFavouriteQuestion(questionId, user);
                return Ok();
            }
            catch (QuestionNotFoundException)
            {
                return BadRequest(new {message = "Question not found"});
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("favourites")]
        public IActionResult DeleteFavouriteQuestion([FromBody] int questionId)
        {
            var user = GetLoggedInUser();
            try
            {
                _questionsService.DeleteFavouriteQuestion(questionId, user);
                return Ok();
            }
            catch (QuestionNotFoundException)
            {
                return BadRequest(new {message = "Question not found"});
            }
            catch (FavouriteNotFoundException)
            {
                return BadRequest(new {message = "This question is not in user favourites"});
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