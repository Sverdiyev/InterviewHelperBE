using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Data;
using Microsoft.Extensions.Options;

namespace InterviewHelper.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;
    
    public UserRepository(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
    }
    
    public User AddUser(User newUser)
    {
        using var context = new InterviewHelperContext(_connectionString);
        context.Users.Add(newUser);
        context.SaveChangesAsync();
        
        return newUser;
    }

    public void EditUserDetails(User user)
    {
        
        using var context = new InterviewHelperContext(_connectionString);
        var userToEdit = context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (userToEdit == null)
        {
            throw new UserNotFoundException();
        }
        userToEdit.Name = user.Name;
        userToEdit.Email = user.Email;
        userToEdit.Password = user.Password;
    }

    public User GetUserById(int userId)
    {
        using var context = new InterviewHelperContext(_connectionString);
        var user = context.Users.FirstOrDefault(u => u.Id == userId);

        return user;
    }

    public User GetUserByEmail(string userEmail)
    {
        using var context = new InterviewHelperContext(_connectionString);
        var user = context.Users.FirstOrDefault(u => u.Email == userEmail);

        return user;
    }

    public User GetUserWithDetails(string userEmail, byte[] userPassword)
    {
        using var context = new InterviewHelperContext(_connectionString);
        var user = context.Users.FirstOrDefault(u => u.Email == userEmail && u.Password == userPassword);

        return user;
    }
}