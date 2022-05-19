using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Data;
using Microsoft.Extensions.Options;

namespace InterviewHelper.Services.Services;

public class QuestionVoteService : IQuestionVoteService
{
    private readonly string _connectionString;

    public QuestionVoteService(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
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
            }
            else
            {
                var newUserFavourite = new Favourite
                {
                    QuestionId = questionId,
                    UserId = authenticatedUser.Id,
                    IsUserFavourite = true
                };

                questionToFavourite.Favourites.Add(newUserFavourite);
            }

            context.SaveChanges();
        }
    }

    public void DeleteFavouriteQuestion(int questionId, User authenticatedUser)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionToDeleteFavourite = context.Questions.FirstOrDefault(_ => _.Id == questionId);
            if (questionToDeleteFavourite == null) throw new QuestionNotFoundException();

            var favouriteExists = context.Favourites.FirstOrDefault(_ =>
                _.QuestionId == questionToDeleteFavourite.Id && _.UserId == authenticatedUser.Id);

            if (favouriteExists == null) throw new FavouriteNotFoundException();

            questionToDeleteFavourite.Favourites.Remove(favouriteExists);
            context.SaveChanges();
        }
    }
}