using System.ComponentModel.DataAnnotations;

namespace InterviewHelper.Core.Models;

public class Vote
{
    private enum VoteOptions
    {
        up,
        down
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    
    [EnumDataType(typeof(VoteOptions))]
    public string? UserVote { get; set; }
}