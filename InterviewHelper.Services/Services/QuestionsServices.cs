using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InterviewHelper.Services.Services;

public class QuestionsServices : IQuestionsServices
{
    private readonly string _connectionString;

    public QuestionsServices(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
    }

    public Question AddQuestion(RequestQuestion newQuestion)
    {
        Question addedQuestion = new Question()
        {
            Complexity = newQuestion.Complexity,
            Note = newQuestion.Note,
            Tags = newQuestion.Tags?.Select(tag => new Tag() {TagName = tag}).ToList() ?? new List<Tag>(),

            Vote = newQuestion.Vote,
            EasyToGoogle = newQuestion.EasyToGoogle ?? true,
            QuestionContent = newQuestion.QuestionContent,
        };

        using (var context = new InterviewHelperContext())
        {
            context.Questions.Add(addedQuestion);
            context.SaveChanges();

            return addedQuestion;
        }
    }

    public List<Question> GetAllQuestions()
    {
        using (var context = new InterviewHelperContext())
        {
            var result = context.Questions.Include("Tags").ToList();
            return result;
        }
    }

    public void UpdateQuestion(int id, RequestQuestion updatedQuestion)
    {
        if (id != updatedQuestion.Id)
        {
            //something wrong with provided question
            throw new Exception();
        }

        using (var context = new InterviewHelperContext())
        {
            var existingQuestion = context.Questions.FirstOrDefault(q => q.Id == updatedQuestion.Id);
            if (existingQuestion != null)
            {
                existingQuestion.Complexity = updatedQuestion.Complexity;
                existingQuestion.Note = updatedQuestion.Note;
                // how to update the tags?
                // existingQuestion.Tags = updatedQuestion.Tags?.Select(tag => new Tag() {TagName = tag}).ToList() ??
                //                         new List<Tag>();
                existingQuestion.Vote = updatedQuestion.Vote;
                existingQuestion.EasyToGoogle = updatedQuestion.EasyToGoogle ?? true;
                existingQuestion.QuestionContent = updatedQuestion.QuestionContent;

                // existingQuestion.Tags.Select(tag => tag.);
            }


            // check for DBupdateConcurrancy error 
            context.SaveChanges();
        }
    }
}