using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionVoteService
{
    void UpVoteQuestion(int questionId, User user);
    void DownVoteQuestion(int questionId, User user);
    void DeleteUserVote(int questionId, User user);
    void AddFavouriteQuestion(int questionId, User user);
    void DeleteFavouriteQuestion(int questionId, User user);
}