namespace InterviewHelper.Models
{
    public class Question
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Language  { get; set; }
        public int Complexity { get; set; }
        public string QuestionContent { get; set; }
        public string? Note { get; set; }
        public int Vote { get; set; }
        public List<string>? Tags {get; set; }
    }
}