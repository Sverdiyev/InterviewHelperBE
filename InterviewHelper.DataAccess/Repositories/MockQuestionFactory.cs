using InterviewHelper.Core.Models;

namespace InterviewHelper.DataAccess.Repositories;

public class MockQuestionFactory
{
    private readonly Random _random = new Random();

    private readonly string[] _questionsContent = new string[]
    {
        "what is your understanding of low level programming languages?",
        "explain polymorphism",
        "explain inheritance",
        "how would you explain a rest api to a non technical person",
        "what sorting algorithms do you know?, explain how one of them works.",
        "what are the scenarios where you would use a non-memory safe language?",
        "explain the difference between post and get requests",
        "explain how client side rendering works",
        "what is the use of the useState hook in React",
        "explain what is a garbage collector? what languages you know use it",
        "explain multithreading",
        "what is React?",
    };

    private readonly string[] _notes = new string[]
    {
        "this is a beginner level question",
        "this question is targeted to senior level positions",
        "this question requires more time",
        "most candidates struggle with this question",
        "this question has trapped a lot of candidates",
        "consider asking this question for highly qualified candidates",
        ""
    };

    private readonly string[] _complexity = new string[] {"hard", "medium", "easy"};
    private readonly int[] _votes = new int[] {90, -20, 80, 20, -19};

    private readonly string[] _tags = new string[]
        {"c++", "python", "java", "OOP", "memory management", "data structures", "frontend", "backend"};

    public Question GenerateQuestion(int id, DateTime timeStamp)
    {
        var questionContent = this._questionsContent[_random.Next(this._questionsContent.Length)];
        var note = this._notes[_random.Next(this._notes.Length)];
        var complexity = this._complexity[_random.Next(this._complexity.Length)];
        var vote = this._votes[_random.Next(this._votes.Length)];
        var tags = new List<string>()
        {
            this._tags[_random.Next(this._tags.Length)],
            this._tags[_random.Next(this._tags.Length - 1)],
        };

        return new Question
        {
            Id = id,
            CreationDate = timeStamp,
            Complexity = complexity,
            QuestionContent = questionContent,
            Note = note,
            Vote = vote,
            Tags = tags
        };
    }
}