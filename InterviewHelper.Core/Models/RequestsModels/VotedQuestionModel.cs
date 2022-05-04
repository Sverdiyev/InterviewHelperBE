using System.Text.Json.Serialization;

namespace InterviewHelper.Core.Models.RequestsModels;

public class VotedQuestionModel
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string Complexity { get; set; }
    public string QuestionContent { get; set; }
    public string Note { get; set; } 
    public int Vote { get; set; }
    public bool EasyToGoogle { get; set; }
    public IList<Tag> Tags { get; set; } = new List<Tag>();
    public string UserVote {get; set; }
}