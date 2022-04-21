using System.Text.Json.Serialization;

namespace InterviewHelper.Core.Models;

public class Tag
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string TagName { get; set; }
}