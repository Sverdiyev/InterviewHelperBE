using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InterviewHelper.Services.Services;

public class QuestionsService : IQuestionsService
{
    private readonly string _connectionString;

    public QuestionsService(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
    }

    public async Task AddQuestion(RequestQuestion newQuestion)
    {
        var addedQuestion = new Question()
        {
            Complexity = newQuestion.Complexity,
            Note = newQuestion.Note,
            Tags = newQuestion.Tags.Select(tag => new Tag() {TagName = tag}).ToList(),
            EasyToGoogle = newQuestion.EasyToGoogle,
            QuestionContent = newQuestion.QuestionContent,
            CreationDate = DateTime.Now,
        };

        using (var context = new InterviewHelperContext(_connectionString))
        {
            context.Questions.Add(addedQuestion);
            await context.SaveChangesAsync();
        }
    }

    public List<Question> GetQuestions(string? rawSearchParam)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            if (string.IsNullOrEmpty(rawSearchParam))
            {
                return context.Questions.Include("Tags").ToList();
            }

            var searchParam = rawSearchParam.ToLower().Trim();

            return context.Questions
                .Where(q => q.Note.ToLower().Contains(searchParam) ||
                            q.QuestionContent.ToLower().Contains(searchParam) ||
                            q.Complexity.ToLower().Contains(searchParam) ||
                            q.Tags.Any(t => t.TagName.ToLower().Contains(searchParam)))
                .Include("Tags")
                .ToList();
        }
    }

    public async Task UpdateQuestion(RequestQuestion updatedQuestion)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var existingQuestion =
                context.Questions.Include("Tags").FirstOrDefault(q => q.Id == updatedQuestion.Id);
            if (existingQuestion == null)
            {
                throw new QuestionNotFoundException();
            }

            existingQuestion.Complexity = updatedQuestion.Complexity;
            existingQuestion.Note = updatedQuestion.Note;
            existingQuestion.EasyToGoogle = updatedQuestion.EasyToGoogle;
            existingQuestion.QuestionContent = updatedQuestion.QuestionContent;
            existingQuestion.Tags.Clear();
            existingQuestion.Tags = updatedQuestion.Tags.Select(tag => new Tag() {TagName = tag}).ToList();

            await context.SaveChangesAsync();
        }
    }

    public void DeleteQuestion(int questionId)
    {
        using var context = new InterviewHelperContext(_connectionString);
        var question = context.Questions.FirstOrDefault(q => q.Id == questionId);
        if (question == null)
        {
            throw new QuestionNotFoundException();
        }

        context.Remove(question);
        context.SaveChanges();
    }
}