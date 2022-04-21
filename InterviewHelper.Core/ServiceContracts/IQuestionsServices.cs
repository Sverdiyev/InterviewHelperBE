using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsServices
{
    Question AddQuestion(RequestQuestion newQuestion);
    List<Question> GetAllQuestions();
    void UpdateQuestion(int id, RequestQuestion editedQuestion);
}