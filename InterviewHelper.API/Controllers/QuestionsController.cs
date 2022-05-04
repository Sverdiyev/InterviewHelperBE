using System.Net;
using System.Security.Claims;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.RequestsModels;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Repositories;
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
        private readonly UserRepository _userRepository;

        public QuestionsController(ILogger<QuestionsController> logger,
            IQuestionsService questionsService, UserRepository userRepository)
        {
            _logger = logger;
            _questionsService = questionsService;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetQuestions(string? search)
        {
            var userSessionEmail = User.FindFirst(ClaimTypes.Email).Value;
            var user = _userRepository.GetUser(userSessionEmail);

            var questions = _questionsService.GetQuestions(search, user);

            return Ok(questions);
        }

        [HttpPost]
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

        [HttpPut]
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

        [HttpPost("Votes/Upvote")]
        public IActionResult UpvoteQuestion(VoteRequest vote)
        {
            var userSessionEmail = User.FindFirst(ClaimTypes.Email).Value;
            var user = _userRepository.GetUser(userSessionEmail);
            if (user.Id != vote.UserId)
            {
                return BadRequest("User is not authorized to perform this action");
            }

            try
            {
                _questionsService.VoteQuestion("up", vote, user);
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

        [HttpPost("Votes/Downvote")]
        public IActionResult DownvoteQuestion(VoteRequest vote)
        {
            var userSessionEmail = User.FindFirst(ClaimTypes.Email).Value;
            var user = _userRepository.GetUser(userSessionEmail);
            if (user.Id != vote.UserId)
            {
                return BadRequest("User is not authorized to perform this action");
            }

            try
            {
                _questionsService.VoteQuestion("down", vote, user);
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

        [HttpDelete("Votes")]
        public IActionResult DeleteQuestionVote(VoteRequest vote)
        {
            var userSessionEmail = User.FindFirst(ClaimTypes.Email).Value;
            var user = _userRepository.GetUser(userSessionEmail);

            if (user.Id != vote.UserId)
            {
                return BadRequest("User is not authorized to perform this action");
            }

            try
            {
                _questionsService.DeleteUserVote(vote, user);
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
    }
}