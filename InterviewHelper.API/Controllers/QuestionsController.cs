using System.Net;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ILogger<QuestionsController> _logger;
        private readonly IQuestionsServices _questionsServices;

        public QuestionsController(IQuestionRepository questionRepository,
            ILogger<QuestionsController> logger, IQuestionsServices questionsServices)
        {
            _questionRepository = questionRepository;
            _logger = logger;
            _questionsServices = questionsServices;
        }

        [HttpGet]
        public IActionResult GetQuestions([FromQuery(Name = "search")] string? searchParam)
        {
            return Ok(_questionsServices.GetQuestions(searchParam));
        }

        [HttpPost]
        public IActionResult PostAddQuestion(RequestQuestion newQuestion)
        {
            try
            {
                return Ok(_questionsServices.AddQuestion(newQuestion));
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
                await _questionsServices.UpdateQuestion(updatedQuestion);
                return NoContent();
            }
            catch (Exception e)
            {
                if (e.Message == "not found")
                    return NotFound();
                return StatusCode((int) HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}