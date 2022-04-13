using System.Net;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Data;
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

        //will need to be moved to a separate controller during separate PR 
        [HttpGet("user")]
        public ActionResult<List<AppUser>> GetUser()
        {
            using (var dbContext = new AppDbContext())
            {
                return Ok(dbContext.AppUsers.ToArray());
            }
        }

        [HttpPost("user")]
        public ActionResult PostAddUser()
        {
            using (var dbContext = new AppDbContext())
            {
                //functionality, data extraction, validation to be added in a separate PR
                dbContext.Add(new AppUser()
                    {Email = "sasha.verdiyev", Name = "Sasha", Rating = 100, Password = "password"});
                dbContext.SaveChanges();
            }

            return Ok();
        }
    }
}