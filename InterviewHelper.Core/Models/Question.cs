namespace InterviewHelper.Models
{
    public class Question
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Language  { get; set; }
        public int Complexity { get; set; }
        public string Content { get; set; }
    }
}