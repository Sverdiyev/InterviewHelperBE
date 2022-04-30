namespace InterviewHelper.Core.Exceptions;

public class UnauthorizedOperationException : Exception
{
    public UnauthorizedOperationException() : base("User is not authorized to perform this operation")
    {
        
    }
}