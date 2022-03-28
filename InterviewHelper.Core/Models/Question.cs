namespace InterviewHelper.Models
{
    public class Question
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Language  { get; set; }
        public string Complexity { get; set; }
        public string QuestionContent { get; set; }
        public string? Note { get; set; }
        public int Vote { get; set; }
        public List<string>? Tags {get; set; }

        public Question(int id, DateTime creationDate, string? language, string complexity, string questionContent, string? note, int vote, List<string>? tags)
        {
            Id = id;
            CreationDate = creationDate;
            Language = language;
            Complexity = complexity;
            QuestionContent = questionContent;
            Note = note;
            Vote = vote;
            Tags = tags;
        }
    }
}