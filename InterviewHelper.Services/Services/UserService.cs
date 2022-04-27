using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;

namespace InterviewHelper.Services.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly HashAlgorithm _sha;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _sha = SHA256.Create();
    }

    public User AddUser(UserDTO newUserDto)
    {
        newUserDto.Password = _sha.ComputeHash(newUserDto.Password);
        var newUser = new User(newUserDto);

        _userRepository.AddUser(newUser);

        return newUser;
    }

    public void EditUser(User user)
    {
        _userRepository.EditUserDetails(user);
    }

    public void CheckAuthority(ClaimsPrincipal authenticatedUser, int userId)
    {
        var dbUser = _userRepository.GetUserByEmail(authenticatedUser.FindFirst(ClaimTypes.Email).Value);

        if (dbUser.Id != userId)
        {
            throw new UnauthorizedOperation();
        }
        
        
        
    }
}