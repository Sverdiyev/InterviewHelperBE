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
        public IActionResult GetAllQuestions()
        {
            return Ok(_questionsServices.GetAllQuestions());
        }

        [HttpPost]
        public IActionResult PostAddQuestion([FromBody] RequestQuestion newQuestion)
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

        [HttpPut("{id}")]
        public IActionResult UpdateQuestion(int id, [FromBody] RequestQuestion updatedQuestion)
        {
            if (id != updatedQuestion.Id) return BadRequest();

            try
            {
                _questionsServices.UpdateQuestion(updatedQuestion);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Not Found") return NotFound();

                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        
    }
}