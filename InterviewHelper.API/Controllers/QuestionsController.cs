using System.Net;
using InterviewHelper.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ILogger<QuestionsController> _logger;


        public QuestionsController(IQuestionRepository questionRepository,
            ILogger<QuestionsController> logger)
        {
            _questionRepository = questionRepository;
            _logger = logger;
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
    }
}