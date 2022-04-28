using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IUserRepository
{
    User AddUser(User newUSer);

    void EditUserDetails(User user);

    User GetUserById(int userId);

    public User GetUserByEmail(string userEmail);

    public User GetUserWithDetails(string userEmail, byte[] userPassword);
}