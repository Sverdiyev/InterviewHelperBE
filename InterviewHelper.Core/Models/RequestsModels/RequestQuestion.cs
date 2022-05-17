using System.ComponentModel.DataAnnotations;

namespace InterviewHelper.Core.Models;

public class RequestQuestion
{
    private enum ComplexityOptions
    {
        easy,
        medium,
        hard
    }
    
    public int? Id { get; set; }

    [EnumDataType(typeof(ComplexityOptions))]
    public string Complexity { get; set; }

    [StringLength(100, MinimumLength = 10)]
    public string QuestionContent { get; set; }
    public string Note { get; set; }
    public List<string> Tags { get; set; }
    public bool HardToGoogle { get; set; }
}

public class QuestionSearchRequest
{
    public string? Search { get; set; }
    public List<string>? Complexity { get; set; }
    public List<string>? Tags { get; set; }
    public List<int>? Vote { get; set; }
    public bool? HardToGoogle { get; set; }
    public bool? Favorite { get; set; }

    public bool IsEmpty
    {
        get
        {
            return Vote == null && Search == null && Complexity == null && Tags == null & HardToGoogle == null &&
                   Favorite == null;
        }
    }
}