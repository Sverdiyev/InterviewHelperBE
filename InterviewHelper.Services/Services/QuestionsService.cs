using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.RequestsModels;
using InterviewHelper.Core.Models.RequestsModels;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Data;
using InterviewHelper.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InterviewHelper.Services.Services;

public class QuestionsService : IQuestionsService
{
    private readonly string _connectionString;
    private readonly CommentRepository _commentRepository;

    public QuestionsService(IOptions<DBConfiguration> config, CommentRepository commentRepository)
    {
        _connectionString = config.Value.ConnectionString;
        _commentRepository = commentRepository;
    }

    public async Task AddQuestion(RequestQuestion newQuestion)
    {
        var addedQuestion = new Question
        {
            Complexity = newQuestion.Complexity,
            Note = newQuestion.Note,
            Tags = newQuestion.Tags.Select(tag => new Tag {TagName = tag}).ToList(),
            EasyToGoogle = newQuestion.EasyToGoogle,
            QuestionContent = newQuestion.QuestionContent,
            CreationDate = DateTime.Now
        };

        using (var context = new InterviewHelperContext(_connectionString))
        {
            context.Questions.Add(addedQuestion);
            await context.SaveChangesAsync();
        }
    }

    public List<VotedQuestionModel> GetQuestions(string? rawSearchParam, int authenticatedUserId)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            if (string.IsNullOrEmpty(rawSearchParam))
            {
                return GetQuestionsWithVotes(context.Questions.Include("Tags").ToList(), authenticatedUserId);
            }

            var searchParam = rawSearchParam.ToLower().Trim();

            var allQuestions = context.Questions
                .Where(q => q.Note.ToLower().Contains(searchParam) ||
                            q.QuestionContent.ToLower().Contains(searchParam) ||
                            q.Complexity.ToLower().Contains(searchParam) ||
                            q.Tags.Any(t => t.TagName.ToLower().Contains(searchParam)))
                .Include("Tags")
                .ToList();

            return GetQuestionsWithVotes(allQuestions, authenticatedUserId);
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
            existingQuestion.Tags = updatedQuestion.Tags.Select(tag => new Tag {TagName = tag}).ToList();

            await context.SaveChangesAsync();
        }
    }

    private List<VotedQuestionModel> GetQuestionsWithVotes(List<Question> questions,
        int authenticatedUserId)
    {
        var questionsWithVotes = new List<VotedQuestionModel>();
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var userVotes = context.Votes.Where(_ => _.UserId == authenticatedUserId).ToList();
            foreach (var question in questions)
            {
                var questionVote = userVotes.FirstOrDefault(_ => _.QuestionId == question.Id);
                if (questionVote == null)
                {
                    questionsWithVotes.Add(new VotedQuestionModel
                    {
                        Id = question.Id,
                        Complexity = question.Complexity,
                        Note = question.Note,
                        EasyToGoogle = question.EasyToGoogle,
                        QuestionContent = question.QuestionContent,
                        UserVote = null,
                        CreationDate = question.CreationDate,
                        Tags = question.Tags,
                        Vote = question.Vote
                    });
                }
                else
                {
                    questionsWithVotes.Add(new VotedQuestionModel
                    {
                        Id = question.Id,
                        Complexity = question.Complexity,
                        Note = question.Note,
                        EasyToGoogle = question.EasyToGoogle,
                        QuestionContent = question.QuestionContent,
                        CreationDate = question.CreationDate,
                        Tags = question.Tags,
                        Vote = question.Vote,
                        UserVote = questionVote.UserVote
                    });
                }
            }
        }

        return questionsWithVotes;
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

    private void VoteQuestion(string userVote, VoteRequest vote, User authenticatedUser)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionToVote = context.Questions.FirstOrDefault(_ => _.Id == vote.QuestionId);
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
                    QuestionId = vote.QuestionId,
                    UserId = vote.UserId,
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

    public void DeleteUserVote(VoteRequest vote, User authenticatedUser)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var questionToDeleteVote = context.Questions.FirstOrDefault(_ => _.Id == vote.QuestionId);
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

    public void UpVoteQuestion(VoteRequest vote, User authenticatedUser)
    {
        VoteQuestion("up", vote, authenticatedUser);
    }

    public void DownVoteQuestion(VoteRequest vote, User authenticatedUser)
    {
        VoteQuestion("down", vote, authenticatedUser);
    }
}