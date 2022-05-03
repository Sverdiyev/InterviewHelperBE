using InterviewHelper.DataAccess.Data;

namespace InterviewHelper.Services.Services;

public static class InitializationService
{
    public static void Init(string connectionString)
    {
        IronPdf.Installation.LinuxAndDockerDependenciesAutoConfig = true;
        using (var context = new InterviewHelperContext(connectionString))
        {
            context.Init();
        }
    }
}