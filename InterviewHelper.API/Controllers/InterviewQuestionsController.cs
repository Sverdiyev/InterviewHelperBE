using InterviewHelper.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InterviewQuestionsController : ControllerBase
    {
        private readonly ILogger<InterviewQuestionsController> _logger;

        public InterviewQuestionsController(ILogger<InterviewQuestionsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAllQuestions")]
        public IEnumerable<Question> Get()
        {
            var questions = new List<Question>();
            // add a few questions
            questions.Add(new Question(1, DateTime.Now, "c#", "medium", "How to create a list in c#?", "", 80,
                new List<string>(){"list in c#"}));
            questions.Add(new Question(2, DateTime.Now, "c++", "hard", "How to allocate memory in c++?", "I hate C/C++ memory management.", -20,
                new List<string>(){"memory management", "low level"}));
            questions.Add(new Question(3, DateTime.Now, "java", "easy", "What is @Override in java?", "This question is for beginners to test basic OOP understanding.", 50,
                new List<string>(){"oop", "inheritance", "polymorphism"}));
            questions.Add(new Question(4, DateTime.Now, "python", "easy", "What is the dict data structure in python?", "", 40,
                new List<string>(){"data structures"}));
            questions.Add(new Question(5, DateTime.Now, "c", "hard", "How can you create you own data type in c?", "This question tests candidates on c core concepts i.e. structs.", -10,
                new List<string>(){"low level","abstraction"}));
            
            return questions;
        }
    }
}