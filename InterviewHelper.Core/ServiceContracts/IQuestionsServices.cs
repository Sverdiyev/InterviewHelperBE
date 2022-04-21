using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsServices
{
    Task<Question> AddQuestion(RequestQuestion newQuestion);
    List<Question> GetAllQuestions();
    void UpdateQuestion(RequestQuestion updatedQuestion);
}