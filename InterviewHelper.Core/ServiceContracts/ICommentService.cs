using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface ICommentService
{
    Comment AddComment(CommentRequest newComment);
    void EditComment(Comment comment);
    void DeleteComment(int commentId);
    User GetCommentOwnerById(int commentId);
}