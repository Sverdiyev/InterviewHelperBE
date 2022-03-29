using System.Linq.Expressions;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.Models;

namespace InterviewHelper.DataAccess.Repositories;

public class QuestionRepository: IQuestionRepository
{
    private readonly QuestionFactory _factory;

    public QuestionRepository(QuestionFactory factory)
    {
        _factory = factory;
    }
    public IEnumerable<Question> GetAllQuestions()
    {
        var questions = new List<Question>();
        var creationTimeStamp = new DateTime(2022, 3, 29, 10,30, 30);

        for (var i = 0; i < 50; i++)
        {
            creationTimeStamp = creationTimeStamp.AddHours(6);
            questions.Add(_factory.GenerateQuestion(i+1, creationTimeStamp));
        }

        return questions;
    }
}