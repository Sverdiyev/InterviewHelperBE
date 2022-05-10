using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.AuthenticationModels;

namespace InterviewHelper.Core.ServiceContracts;

public interface IUserService
{
    AuthenticateUserResponse AddUser(UserRequest newUser);
    void EditUser(UserUpdateRequest user);
    public AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest user);
    User GetUserByEmail(string email);
}