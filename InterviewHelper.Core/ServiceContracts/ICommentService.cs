using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface ICommentService
{
    IEnumerable<CommentResponse> GetAllQuestionComments(int questionId);
    Comment AddComment(CommentRequest newComment, int userId);
    void EditComment(Comment comment);
    void DeleteComment(int commentId);
    User GetCommentOwnerById(int commentId);
    bool CommentBelongsToUser(string email, int commentId);
}