using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.RequestsModels;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsService
{
    Task AddQuestion(RequestQuestion newQuestion);
    Task UpdateQuestion(RequestQuestion updatedQuestion);
    void DeleteQuestion(int questionId);
    List<string> GetQuestionsByIds(List<int> questionIds);
    IEnumerable<QuestionActionsModel> GetQuestionsWithSearch(QuestionSearchRequest searchParams, int userId);
    List<string> GetQuestionsTags();
    Question GetQuestionById(int questionId);
    public bool CheckIfQuestionExists(int questionId);
}