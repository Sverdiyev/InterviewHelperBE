using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.RequestsModels;
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
        var addedQuestion = new Question
        {
            Complexity = newQuestion.Complexity,
            Note = newQuestion.Note,
            Tags = newQuestion.Tags.Select(tag => new Tag() {TagName = tag}).ToList(),
            HardToGoogle = newQuestion.HardToGoogle,
            QuestionContent = newQuestion.QuestionContent,
            CreationDate = DateTime.Now
        };

        using (var context = new InterviewHelperContext(_connectionString))
        {
            context.Questions.Add(addedQuestion);
            await context.SaveChangesAsync();
        }
    }

    public IEnumerable<QuestionActionsModel> GetQuestionsWithSearch(QuestionSearchRequest searchParams,
        int authenticatedUserId)
    {
        if (searchParams.IsEmpty)
        {
            return GetQuestionsWithTagsAndUserVoteAndUserFavourite(authenticatedUserId);
        }

        var allQuestions = GetQuestionsWithTagsAndUserVoteAndUserFavourite(authenticatedUserId);
        var filteredQuestions = allQuestions
            .Where(_ => searchParams.Complexity == null || searchParams.Complexity.Contains(_.Complexity))
            .Where(_ => searchParams.Tags == null ||
                        _.Tags.Any(tag => searchParams.Tags.Contains(tag.TagName)))
            .Where(_ => searchParams.HardToGoogle == null || _.HardToGoogle == searchParams.HardToGoogle)
            .Where(_ => searchParams.QuestionRating == null ||
                        _.Vote > searchParams.QuestionRating.Min() && _.Vote < searchParams.QuestionRating.Max())
            .Where(_ => searchParams.Search == null ||
                        _.Note.ToLower().Contains(searchParams.Search) ||
                        _.QuestionContent.ToLower().Contains(searchParams.Search) ||
                        _.Complexity.ToLower().Contains(searchParams.Search) ||
                        _.Tags.Any(_ => _.TagName.ToLower().Contains(searchParams.Search)))
            .Where(_ => searchParams.Favorite == null || _.IsUserFavourite );


        return filteredQuestions;
    }

    public List<string> GetQuestionsTags()
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            return context.Tags.Where(_ => _.TagName != string.Empty).Select(_ => _.TagName).Distinct().ToList();
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
            existingQuestion.Tags = updatedQuestion.Tags.Select(tag => new Tag {TagName = tag}).ToList();

            await context.SaveChangesAsync();
        }
    }

    private IEnumerable<QuestionActionsModel> GetQuestionsWithTagsAndUserVoteAndUserFavourite(int userId)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questions = context.Questions.Include(_ => _.Tags)
                .Include(_ => _.Votes)
                .Include(_ => _.Favourites)
                .ToList()
                .Select(_ => new QuestionActionsModel
                {
                    Id = _.Id,
                    Complexity = _.Complexity,
                    Note = _.Note,
                    HardToGoogle = _.HardToGoogle,
                    QuestionContent = _.QuestionContent,
                    CreationDate = _.CreationDate,
                    Tags = _.Tags,
                    Vote = _.Vote,
                    UserVote = _.Votes.Any(_ => _.UserId == userId)
                        ? _.Votes.First(_ => _.UserId == userId).UserVote
                        : null,
                    IsUserFavourite = _.Favourites.Any(_ => _.UserId == userId)
                        ? _.Favourites.First(_ => _.UserId == userId).IsUserFavourite
                        : false,
                });

            return questions.OrderByDescending(_ => _.CreationDate);
        }
    }

    public void DeleteQuestion(int questionId)
    {
        using var context = new InterviewHelperContext(_connectionString);

        var question = GetQuestionById(questionId);

        var questionVotes = context.Votes.Where(_ => _.QuestionId == questionId).ToList();
        context.RemoveRange(questionVotes);

        context.Remove(question);
        context.SaveChanges();
    }

    public Question GetQuestionById(int questionId)
    {
        using var context = new InterviewHelperContext(_connectionString);
        var question = context.Questions.FirstOrDefault(_ => _.Id == questionId);
        if (question == null)
        {
            throw new QuestionNotFoundException();
        }

        return question;
    }

    public List<Question> GetQuestionsByIds(List<int> questionIds)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionsContents = context.Questions.Where(_ => questionIds.Contains(_.Id))
                .ToList();
            return questionsContents;
        }
    }

    public bool CheckIfQuestionExists(int questionId)
    {
        using var context = new InterviewHelperContext(_connectionString);
        var question = context.Questions.FirstOrDefault(_ => _.Id == questionId);

        return question != null;
    }
}