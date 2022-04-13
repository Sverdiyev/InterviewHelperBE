using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts
{
    public interface IQuestionRepository
    {
        IEnumerable<Question> GetAllQuestions();
    }
}