using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.RequestsModels;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsService
{
    Task AddQuestion(RequestQuestion newQuestion);
    List<VotedQuestionModel> GetQuestions(string? searchParam, int userId);
    Task UpdateQuestion(RequestQuestion updatedQuestion);
    void DeleteQuestion(int questionId);
    List<string> GetQuestionsByIds(List<int> questionIds);
    void UpVoteQuestion(VoteRequest vote, User user);
    void DownVoteQuestion(VoteRequest vote, User user);
    void DeleteUserVote(VoteRequest vote, User user);
}