using System.Text;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.DataAccess.Data;
using Microsoft.Extensions.Options;

namespace InterviewHelper.DataAccess.Repositories;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
    }

    public User AddUser(User newUser)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            context.Users.Add(newUser);
            context.SaveChanges();

            return newUser;
        }
    }

    public void EditUserDetails(UserUpdateRequest user)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            var userToEdit = context.Users.First(u => u.Id == user.Id);

            userToEdit.Name = user.Name;
            userToEdit.Password = user.Password;

            context.SaveChanges();
        }
    }

    public bool UserExists(string email)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            return context.Users.Any(u => u.Email == email);
        }
    }

    public bool ValidUser(string email, string encryptedPassword)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            return context.Users.Any(u => u.Email == email && u.Password == encryptedPassword);
        }
    }

    public User GetUser(string email)
    {
        using (var context = new InterviewHelperContext(_connectionString))
        {
            return context.Users.First(_ => _.Email == email);
        }
    }
}