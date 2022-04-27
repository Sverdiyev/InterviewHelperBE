namespace InterviewHelper.Core.Exceptions;

public class UnauthorizedOperation : Exception
{
    public UnauthorizedOperation() : base("User is not authorized to perform this operation")
    {
        
    }
}