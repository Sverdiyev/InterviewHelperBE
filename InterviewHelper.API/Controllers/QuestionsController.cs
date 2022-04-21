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

        [HttpGet("allQuestions")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_questionRepository.GetAllQuestions());
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("questions")]
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

        [HttpGet("questions")]
        public IActionResult GetAllQuestions()
        {
            return Ok(_questionsServices.GetAllQuestions());
        }
        
        
    }
}