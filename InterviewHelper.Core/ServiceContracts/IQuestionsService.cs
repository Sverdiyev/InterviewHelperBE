using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.RequestsModels;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsService
{
    Task AddQuestion(RequestQuestion newQuestion);
    Task UpdateQuestion(RequestQuestion updatedQuestion);
    void DeleteQuestion(int questionId);
    List<string> GetQuestionsByIds(List<int> questionIds);
    IEnumerable<VotedQuestionModel> GetQuestionsWithSearch(RequestQuestionSearch searchParams, int userId);
    List<Tag> GetQuestionsTags();
    void UpVoteQuestion(VoteRequest vote, User user);
    void DownVoteQuestion(VoteRequest vote, User user);
    void DeleteUserVote(VoteRequest vote, User user);
    Question GetQuestionById(int questionId);
    public bool CheckIfQuestionExists(int questionId);

}