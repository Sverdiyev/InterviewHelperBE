namespace InterviewHelper.Core.Models;

public class Favourite
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public bool IsUserFavourite { get; set; }
}