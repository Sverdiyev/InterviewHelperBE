using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Data;
using Microsoft.Extensions.Options;

namespace InterviewHelper.Services.Services;

public class QuestionsServices : IQuestionsServices
{
    private readonly string _connectionString;

    public QuestionsServices(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
    }

    public void AddQuestion()
    {
        throw new NotImplementedException();
    }
}