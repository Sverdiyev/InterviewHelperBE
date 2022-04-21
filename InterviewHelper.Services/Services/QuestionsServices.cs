using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InterviewHelper.Services.Services;

public class QuestionsServices : IQuestionsServices
{
    private readonly string _connectionString;

    public QuestionsServices(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
    }

    public Question AddQuestion(RequestQuestion newQuestion)
    {
        using (var context = new InterviewHelperContext())
        {
            Question addedQuestion = new Question()
            {
                Complexity = newQuestion.Complexity,
                Note = newQuestion.Note,
                Tags = newQuestion.Tags.Select(tag => new Tag() {TagName = tag}).ToList(),
                Vote = newQuestion.Vote,
                EasyToGoogle = newQuestion.EasyToGoogle,
                QuestionContent = newQuestion.QuestionContent,
            };
            context.Questions.Add(addedQuestion);
            context.SaveChanges();

            return addedQuestion;
        }
    }

    public List<Question> GetAllQuestions()
    {
        using (var context = new InterviewHelperContext())
        {
            var result = context.Questions.Include("Tags").ToList();
            return result;
        }
    }
}