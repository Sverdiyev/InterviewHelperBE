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
            .Where(_ => searchParams.Vote == null ||
                        _.Vote > searchParams.Vote.Min() && _.Vote < searchParams.Vote.Max())
            .Where(_ => searchParams.Search == null ||
                        _.Note.ToLower().Contains(searchParams.Search) ||
                        _.QuestionContent.ToLower().Contains(searchParams.Search) ||
                        _.Complexity.ToLower().Contains(searchParams.Search) ||
                        _.Tags.Any(_ => _.TagName.ToLower().Contains(searchParams.Search)))
            .Where(_ => searchParams.Complexity == null || _.IsUserFavourite == searchParams.Favorite);


        return filteredQuestions.OrderByDescending(_ => _.CreationDate);
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

            return questions;
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

    public List<string> GetQuestionsByIds(List<int> questionIds)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionsContents = context.Questions.Where(_ => questionIds.Contains(_.Id))
                .Select(_ => _.QuestionContent).ToList();
            return questionsContents;
        }
    }

    public bool CheckIfQuestionExists(int questionId)
    {
        using var context = new InterviewHelperContext(_connectionString);
        var question = context.Questions.FirstOrDefault(_ => _.Id == questionId);

        return question != null;
    }

    private void VoteQuestion(string userVote, int questionId, User authenticatedUser)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionToVote = context.Questions.FirstOrDefault(_ => _.Id == questionId);
            if (questionToVote == null) throw new QuestionNotFoundException();

            var voteExists = context.Votes.FirstOrDefault(_ =>
                _.QuestionId == questionToVote.Id && _.UserId == authenticatedUser.Id);
            if (voteExists != null)
            {
                if (voteExists.UserVote == userVote)
                {
                    return;
                }

                var voteValue = userVote == "up"
                    ? questionToVote.Vote += 2
                    : questionToVote.Vote -= 2;

                voteExists.UserVote = userVote;

                context.SaveChanges();
            }
            else
            {
                var newUserVote = new Vote
                {
                    QuestionId = questionId,
                    UserId = authenticatedUser.Id,
                    UserVote = userVote
                };

                var voteValue = userVote == "up"
                    ? questionToVote.Vote += 1
                    : questionToVote.Vote -= 1;

                context.Votes.Add(newUserVote);
                context.SaveChanges();
            }
        }
    }

    public void DeleteUserVote(int questionId, User authenticatedUser)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionToDeleteVote = context.Questions.FirstOrDefault(_ => _.Id == questionId);
            if (questionToDeleteVote == null)
            {
                throw new QuestionNotFoundException();
            }

            var voteExists = context.Votes.FirstOrDefault(_ =>
                _.QuestionId == questionToDeleteVote.Id && _.UserId == authenticatedUser.Id);
            if (voteExists == null)
            {
                throw new VoteNotFoundException();
            }

            var voteValue = voteExists.UserVote == "up"
                ? questionToDeleteVote.Vote -= 1
                : questionToDeleteVote.Vote += 1;

            context.Votes.Remove(voteExists);
            context.SaveChanges();
        }
    }

    public void UpVoteQuestion(int questionId, User authenticatedUser)
    {
        VoteQuestion("up", questionId, authenticatedUser);
    }

    public void DownVoteQuestion(int questionId, User authenticatedUser)
    {
        VoteQuestion("down", questionId, authenticatedUser);
    }

    public void AddFavouriteQuestion(int questionId, User authenticatedUser)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionToFavourite = context.Questions.FirstOrDefault(_ => _.Id == questionId);
            if (questionToFavourite == null) throw new QuestionNotFoundException();

            var favouriteExists = context.Favourites.FirstOrDefault(_ =>
                _.QuestionId == questionToFavourite.Id && _.UserId == authenticatedUser.Id);

            if (favouriteExists != null)
            {
                favouriteExists.IsUserFavourite = true;
                context.SaveChanges();
            }

            var newUserFavourite = new Favourite
            {
                QuestionId = questionId,
                UserId = authenticatedUser.Id,
                IsUserFavourite = true
            };

            context.Favourites.Add(newUserFavourite);
            context.SaveChanges();
        }
    }

    public void DeleteFavouriteQuestion(int questionId, User authenticatedUser)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionToFavourite = context.Questions.FirstOrDefault(_ => _.Id == questionId);
            if (questionToFavourite == null) throw new QuestionNotFoundException();

            var favouriteExists = context.Favourites.FirstOrDefault(_ =>
                _.QuestionId == questionToFavourite.Id && _.UserId == authenticatedUser.Id);

            if (favouriteExists == null) throw new FavouriteNotFoundException();

            context.Favourites.Remove(favouriteExists);
            context.SaveChanges();
        }
    }
}