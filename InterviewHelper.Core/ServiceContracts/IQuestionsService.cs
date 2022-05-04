using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.RequestsModels;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsService
{
    Task AddQuestion(RequestQuestion newQuestion);
    List<VotedQuestionModel> GetQuestions(string? searchParam, User user);
    Task UpdateQuestion(RequestQuestion updatedQuestion);
    void DeleteQuestion(int questionId);
    List<string> GetQuestionsByIds(List<int> questionIds);
    void VoteQuestion(string userVote, VoteRequest vote, User user);
    void DeleteUserVote(VoteRequest vote, User user);
}