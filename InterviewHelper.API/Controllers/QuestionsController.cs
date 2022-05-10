using System.Net;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly ILogger<QuestionsController> _logger;
        private readonly IQuestionsService _questionsService;

        public QuestionsController(ILogger<QuestionsController> logger,
            IQuestionsService questionsService)
        {
            _logger = logger;
            _questionsService = questionsService;
        }

        [HttpPost("/fetch-questions")]
        public IActionResult PostQuestionSearch(RequestQuestionSearch searchParams)
        {
            return Ok(_questionsService.GetQuestionsWithSearch(searchParams));
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
    }
}