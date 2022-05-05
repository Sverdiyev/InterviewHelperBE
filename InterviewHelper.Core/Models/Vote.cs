namespace InterviewHelper.Core.Models;

public class Vote
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public string? UserVote { get; set; }
}