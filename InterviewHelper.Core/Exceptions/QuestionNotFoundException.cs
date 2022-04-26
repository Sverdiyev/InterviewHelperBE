namespace InterviewHelper.Core.Exceptions;

public class QuestionNotFoundException : Exception
{
    public QuestionNotFoundException() : base("Question not found")
    {
    }
}