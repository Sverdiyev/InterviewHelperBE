using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsService
{
    Task AddQuestion(RequestQuestion newQuestion);
    List<Question> GetQuestions(string? searchParam);
    Task UpdateQuestion(RequestQuestion updatedQuestion);
    void DeleteQuestion(int questionId);
    List<string> GetQuestionsByIds(List<int> questionIds);
    Question GetQuestionById(int questionId);
    public bool CheckIfQuestionExists(int questionId);

}