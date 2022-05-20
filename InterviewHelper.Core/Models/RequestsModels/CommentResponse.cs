namespace InterviewHelper.Core.Models;

public class CommentResponse
{
    public int Id { get; set; }
    public string CommentContent { get; set; }
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public DateTime CreationDate { get; set; }
}