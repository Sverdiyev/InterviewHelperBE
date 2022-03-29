using System.Net;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Repositories;
using InterviewHelper.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InterviewQuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ILogger<InterviewQuestionsController> _logger;

        public InterviewQuestionsController(IQuestionRepository questionRepository, ILogger<InterviewQuestionsController> logger)
        {
            _questionRepository = questionRepository;
            _logger = logger;
        }

        [HttpGet(Name = "GetAllQuestions")]
        public IActionResult Get()
        {
            try 
            {
                return Ok(_questionRepository.GetAllQuestions());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}