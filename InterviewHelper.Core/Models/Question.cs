using System.Text.Json.Serialization;

namespace InterviewHelper.Core.Models
{
    public class Question
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string Complexity { get; set; }
        public string QuestionContent { get; set; }
        public string? Note { get; set; }
        public int Vote { get; set; }
        public bool EasyToGoogle { get; set; }
        public virtual List<Tag>? Tags { get; set; }
    }
}