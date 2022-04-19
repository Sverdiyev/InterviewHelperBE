using InterviewHelper.DataAccess.Data;

namespace InterviewHelper.Services.Services;

public static class InitializationService
{
    public static void Init(string connectionString)
    {
        using (var context = new InterviewHelperContext())
        {
            context.Init();
        }
    }
    
}