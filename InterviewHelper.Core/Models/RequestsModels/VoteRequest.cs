using System.ComponentModel.DataAnnotations;

namespace InterviewHelper.Core.Models.RequestsModels;

public class VoteRequest
{
    public int UserId { get; set; }
    public int QuestionId { get; set; }
}