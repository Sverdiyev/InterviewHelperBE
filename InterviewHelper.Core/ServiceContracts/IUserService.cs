using System.Security.Claims;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.AuthenticationModels;

namespace InterviewHelper.Core.ServiceContracts;

public interface IUserService
{
    AuthenticateUserResponse AddUser(UserRequest newUser);
    void EditUser(UserUpdateRequest user);
    public AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest user);
    public void CheckAuthority(ClaimsPrincipal authenticatedUser, int userId);
    public string GenerateJwtToken(User user);
    public void CheckIfEmailExists(string userEmail);
}