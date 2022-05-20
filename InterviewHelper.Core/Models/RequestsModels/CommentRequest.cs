namespace InterviewHelper.Core.Models;

public class CommentRequest
{
    public string CommentContent { get; set; }
    public int QuestionId { get; set; }
    public DateTime CreationDate { get; set; }

}