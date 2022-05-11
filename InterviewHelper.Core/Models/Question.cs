namespace InterviewHelper.Core.Models
{
    public class Question
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Complexity { get; set; }
        public string QuestionContent { get; set; }
        public string Note { get; set; } 
        public int Vote { get; set; }
        public bool HardToGoogle { get; set; }
        public virtual IList<Tag> Tags { get; set; }
        public virtual IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}