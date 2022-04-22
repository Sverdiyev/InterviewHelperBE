namespace InterviewHelper.Core.Models
{
    public class Question
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string Complexity { get; set; } = string.Empty;
        public string QuestionContent { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public int Vote { get; set; } 
        public bool EasyToGoogle { get; set; }
        public virtual List<Tag> Tags { get; set; } 
    }
}