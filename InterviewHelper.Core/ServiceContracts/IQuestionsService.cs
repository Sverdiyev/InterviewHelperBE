using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IQuestionsService
{
    Task AddQuestion(RequestQuestion newQuestion);
    Task UpdateQuestion(RequestQuestion updatedQuestion);
    void DeleteQuestion(int questionId);
    List<string> GetQuestionsByIds(List<int> questionIds);
    List<Question> GetQuestionsWithSearch(RequestQuestionSearch searchParams);
    List<Tag> GetQuestionTags();
}