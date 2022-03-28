using InterviewHelper.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InterviewQuestionsController : ControllerBase
    {
        private static readonly string[] ProgrammingLanguages = new[]
        {
            "C#", "Java", "Javascript", "C++", "Python", "Go"
        };

        private readonly ILogger<InterviewQuestionsController> _logger;

        public InterviewQuestionsController(ILogger<InterviewQuestionsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAllQuestions")]
        public IEnumerable<Question> Get()
        {
            return Enumerable.Range(0, 4).Select(index => new Question
            {
                Id = index,
                CreationDate = DateTime.Now.AddDays(index),
                Complexity = 0,
                Language = ProgrammingLanguages[index],
                QuestionContent = $"This question is about '{ProgrammingLanguages[index]}' programming language!"
            })
            .ToArray();
        }
    }
}