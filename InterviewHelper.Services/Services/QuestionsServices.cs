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

    public async Task<Question> AddQuestion(RequestQuestion newQuestion)
    {
        var addedQuestion = new Question()
        {
            Complexity = newQuestion.Complexity,
            Note = newQuestion.Note,
            Tags = newQuestion.Tags.Select(tag => new Tag() {TagName = tag}).ToList(),
            EasyToGoogle = newQuestion.EasyToGoogle,
            QuestionContent = newQuestion.QuestionContent,
        };

        using (var context = new InterviewHelperContext())
        {
            context.Questions.Add(addedQuestion);
            await context.SaveChangesAsync();

            return addedQuestion;
        }
    }

    public List<Question> GetQuestions(string? rawSearchParam)
    {
        using (var context = new InterviewHelperContext())
        {
            if (!string.IsNullOrEmpty(rawSearchParam))
            {
                return context.Questions.Include("Tags").ToList();
            }

            var searchParam = rawSearchParam.ToLower().Trim();

            return context.Questions
                .Where(q => q.Note.ToLower().Contains(searchParam) ||
                            q.QuestionContent.ToLower().Contains(searchParam) ||
                            (q.EasyToGoogle && searchParam == "easy to google") ||
                            q.Complexity.ToLower().Contains(searchParam) ||
                            q.Tags.Any(t => t.TagName.ToLower().Contains(searchParam)))
                .Include("Tags")
                .ToList();
        }
    }

    public async Task UpdateQuestion(RequestQuestion updatedQuestion)
    {
        using (var context = new InterviewHelperContext())
        {
            var existingQuestion =
                context.Questions.Include("Tags").FirstOrDefault(q => q.Id == updatedQuestion.Id);
            if (existingQuestion != null)
            {
                existingQuestion.Complexity = updatedQuestion.Complexity;
                existingQuestion.Note = updatedQuestion.Note;
                existingQuestion.Vote = updatedQuestion.Vote;
                existingQuestion.EasyToGoogle = updatedQuestion.EasyToGoogle;
                existingQuestion.QuestionContent = updatedQuestion.QuestionContent;
                existingQuestion.Tags.Clear();
                existingQuestion.Tags = updatedQuestion.Tags.Select(tag => new Tag() {TagName = tag}).ToList();
            }
            else
            {
                throw new QuestionNotFoundException();
            }

            await context.SaveChangesAsync();
        }
    }
}

public class QuestionNotFoundException : Exception
{
    public QuestionNotFoundException() : base("Question not found")
    {
    }
}