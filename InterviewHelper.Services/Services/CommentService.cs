using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Repositories;

namespace InterviewHelper.Services.Services;

public class CommentService : ICommentService
{
    private readonly CommentRepository _commentRepository;

    public CommentService(CommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public Comment AddComment(CommentRequest newComment)
    {
        var commentToAdd = new Comment
        {
            CommentContent = newComment.CommentContent,
            UserId = newComment.UserId,
            QuestionId = newComment.QuestionId
        };
        return _commentRepository.AddComment(commentToAdd);
    }

    public void EditComment(Comment comment)
    {
        _commentRepository.EditCommentContent(comment);
    }

    public void DeleteComment(int commentId)
    {
        _commentRepository.DeleteComment(commentId);
    }

    public User GetCommentOwnerById(int commentId)
    {
        var commentOwner = _commentRepository.GetCommentOwnerById(commentId);
        if (commentOwner == null)
        {
            throw new UserNotFoundException();
        }

        return commentOwner;
    }
}