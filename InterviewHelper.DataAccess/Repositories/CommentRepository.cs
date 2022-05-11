using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.DataAccess.Data;
using Microsoft.Extensions.Options;

namespace InterviewHelper.DataAccess.Repositories;

public class CommentRepository
{
    private readonly string _connectionString;

    public CommentRepository(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
    }

    public List<Comment> GetAllQuestionComments(int questionId)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var commentQuestions = context.Comments.Where(_ => _.QuestionId == questionId).ToList();
            return commentQuestions;
        }
    }

    public Comment AddComment(Comment newComment)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            context.Comments.Add(newComment);
            context.SaveChanges();

            return newComment;
        }
    }

    public void EditCommentContent(Comment comment)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var commentToEdit = context.Comments.FirstOrDefault(_ => _.Id == comment.Id);
            if (commentToEdit == null)
            {
                throw new CommentNotFoundException();
            }

            commentToEdit.CommentContent = comment.CommentContent;
            context.SaveChanges();
        }
    }

    public void DeleteComment(int commentId)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var commentToDelete = context.Comments.FirstOrDefault(_ => _.Id == commentId);
            if (commentToDelete == null)
            {
                throw new CommentNotFoundException();
            }

            context.Remove(commentToDelete);
            context.SaveChanges();
        }
    }

    public User GetCommentOwnerById(int commentId)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var commentToEdit = context.Comments.FirstOrDefault(_ => _.Id == commentId);
            if (commentToEdit == null)
            {
                throw new CommentNotFoundException();
            }
            var commentOwnerId = context.Comments.Where(_ => _.Id == commentId).Select(_ => _.UserId).FirstOrDefault();
            
            return context.Users.FirstOrDefault(_ => commentOwnerId == _.Id);
        }
    }
}