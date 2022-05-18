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
    void UpVoteQuestion(int questionId, User user);
    void DownVoteQuestion(int questionId, User user);
    void DeleteUserVote(int questionId, User user);
    Question GetQuestionById(int questionId);
    public bool CheckIfQuestionExists(int questionId);
    void AddFavouriteQuestion(int questionId, User user);
    void DeleteFavouriteQuestion(int questionId, User user);

}