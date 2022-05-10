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
            HardToGoogle = newQuestion.HardToGoogle,
            QuestionContent = newQuestion.QuestionContent,
            CreationDate = DateTime.Now,
        };

        using (var context = new InterviewHelperContext(_connectionString))
        {
            context.Questions.Add(addedQuestion);
            await context.SaveChangesAsync();
        }
    }

    public List<Question> GetQuestionsWithSearch(RequestQuestionSearch searchParams)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            if (searchParams.IsEmpty)
            {
                return context.Questions.Include("Tags").ToList();
            }

            //TODO:  to add filter by Favorite once it is added
            return context.Questions
                .Where(q => searchParams.Complexity == null || searchParams.Complexity.Contains(q.Complexity))
                .Where(q => searchParams.Tags == null ||
                            q.Tags.Any(tag => searchParams.Tags.Contains(tag.TagName)))
                .Where(q => searchParams.HardToGoogle == null || q.HardToGoogle == searchParams.HardToGoogle)
                .Where(q => searchParams.Vote == null ||
                            q.Vote > searchParams.Vote.Min() && q.Vote < searchParams.Vote.Max())
                .Where(q => searchParams.Search == null ||
                            q.Note.ToLower().Contains(searchParams.Search) ||
                            q.QuestionContent.ToLower().Contains(searchParams.Search) ||
                            q.Complexity.ToLower().Contains(searchParams.Search) ||
                            q.Tags.Any(t => t.TagName.ToLower().Contains(searchParams.Search)))
                .Include("Tags").ToList();
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
            existingQuestion.HardToGoogle = updatedQuestion.HardToGoogle;
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

    public List<string> GetQuestionsByIds(List<int> questionIds)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionsContents = context.Questions.Where(_ => questionIds.Contains(_.Id))
                .Select(_ => _.QuestionContent).ToList();
            return questionsContents;
        }
    }
}