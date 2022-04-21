namespace InterviewHelper.Core.Models;

public class RequestQuestion
{
    public string Complexity { get; set; }
    public string QuestionContent { get; set; }
    public string? Note { get; set; }
    public int Vote { get; set; }
    public List<string>? Tags { get; set; }
    public bool EasyToGoogle { get; set; }
}
