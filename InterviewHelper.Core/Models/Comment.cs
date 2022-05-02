namespace InterviewHelper.Core.Models;

public class Comment
{
    public int Id { get; set; }
    public string CommentContent { get; set; }
    public int QuestionId { get; set; }
    public int UserId { get; set; }
}