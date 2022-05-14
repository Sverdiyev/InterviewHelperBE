using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.RequestsModels;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsService
{
    Task AddQuestion(RequestQuestion newQuestion);
    IEnumerable<QuestionActionsModel> GetQuestions(string? searchParam, int userId);
    Task UpdateQuestion(RequestQuestion updatedQuestion);
    void DeleteQuestion(int questionId);
    List<string> GetQuestionsByIds(List<int> questionIds);
    void UpVoteQuestion(QuestionActionsRequest questionActions, User user);
    void DownVoteQuestion(QuestionActionsRequest questionActions, User user);
    void DeleteUserVote(QuestionActionsRequest questionActions, User user);
    Question GetQuestionById(int questionId);
    public bool CheckIfQuestionExists(int questionId);
    void AddFavouriteQuestion(QuestionActionsRequest questionActions, User user);
    void DeleteFavouriteQuestion(QuestionActionsRequest questionActions, User user);

}