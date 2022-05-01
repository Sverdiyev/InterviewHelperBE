using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsService
{
    Task AddQuestion(RequestQuestion newQuestion);
    List<Question> GetQuestions(string? searchParam);
    Task UpdateQuestion(RequestQuestion updatedQuestion);
    void DeleteQuestion(int questionId);
}