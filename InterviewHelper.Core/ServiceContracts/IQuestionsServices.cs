using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsServices
{
    void AddQuestion(RequestQuestion newQuestion);
    List<Question> GetQuestions(string? searchParam);
    Task UpdateQuestion(RequestQuestion updatedQuestion);
}