using InterviewHelper.Core.Models;

namespace InterviewHelper.Core.ServiceContracts;

public interface IUserRepository
{
    User AddUser(User newUser);

    void EditUserDetails(UserUpdateRequest user);

    User GetUserById(int userId);

    public User GetUserByEmail(string userEmail);

    public User GetUserWithDetails(string userEmail, byte[] userPassword);
}