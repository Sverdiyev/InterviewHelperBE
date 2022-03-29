using System.Linq.Expressions;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.Models;

namespace InterviewHelper.DataAccess.Repositories;

public class QuestionRepository: IQuestionRepository
{
    private readonly QuestionFactory _factory;
    private readonly List<Question> _questions;

    public QuestionRepository()
    {
        _questions = new List<Question>();
        _factory = new QuestionFactory();
    }
    public IEnumerable<Question> GetAllQuestions()
    {
        var creationTimeStamp = new DateTime(2022, 3, 29);

        for (var i = 0; i < 50; i++)
        {
            _questions.Add(_factory.GenerateQuestion(i+1, creationTimeStamp.AddDays(1)));
        }

        return _questions;
    }
}