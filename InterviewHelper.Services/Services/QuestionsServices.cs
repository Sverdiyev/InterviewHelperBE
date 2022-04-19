using InterviewHelper.Core.Models;
using InterviewHelper.DataAccess.Data;
using Microsoft.Extensions.Options;

namespace InterviewHelper.Services.Services;

public class QuestionsServices
{
    private readonly string _connectionString;

    public QuestionsServices(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
    }

    public void AddQuestion()
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            //add logic here
        }
    }
}