namespace InterviewHelper.Core.Models.RequestsModels;

public class QuestionActionsModel
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string Complexity { get; set; }
    public string QuestionContent { get; set; }
    public string Note { get; set; } 
    public int Vote { get; set; }
    public bool HardToGoogle { get; set; }
    public IList<Tag> Tags { get; set; } = new List<Tag>();
    public string UserVote {get; set; }
    public bool IsUserFavourite { get; set; }
}