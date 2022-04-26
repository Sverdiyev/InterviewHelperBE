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
    public int Vote { get; set; }
    public List<string> Tags { get; set; }
    public bool EasyToGoogle { get; set; }
}
