using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Helper;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.AuthenticationModels;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InterviewHelper.Services.Services;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly HashAlgorithm _sha;
    private readonly string _secret;

    public UserService(UserRepository userRepository, IOptions<AuthenticationSecret> configuration)
    {
        _userRepository = userRepository;
        _sha = SHA256.Create();
        _secret = configuration.Value.Secret;
    }

    public AuthenticateUserResponse AddUser(UserRequest user)
    {
        if (_userRepository.UserExists(user.Email))
            throw new Exception("User with the same email already exists.");

        var byteVersionPassword = Encoding.ASCII.GetBytes(user.Password);

        user.Password = Encoding.ASCII.GetString(_sha.ComputeHash(byteVersionPassword));

        var newUser = new User
        {
            CreationDate = DateTime.Now,
            Email = user.Email,
            Name = user.Name,
            Password = user.Password
        };

        _userRepository.AddUser(newUser);

        var token = GenerateJwtToken(newUser);

        return new AuthenticateUserResponse
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Email = newUser.Email,
            Token = token
        };
    }

    public void EditUser(UserUpdateRequest user)
    {
        if (!_userRepository.UserExists(user.Email))
            throw new UserNotFoundException();

        _userRepository.EditUserDetails(user);
    }

    public AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest userRequest)
    {
        if (!_userRepository.UserExists(userRequest.Email))
            throw new Exception("Given user does not exists");
        
        var byteVersionPassword = Encoding.ASCII.GetBytes(userRequest.Password);
        var encryptedPassword = Encoding.ASCII.GetString(_sha.ComputeHash(byteVersionPassword));

        var successfullLogIn = _userRepository.ValidUser(userRequest.Email, encryptedPassword);

        if (!successfullLogIn)
            throw new AuthenticationFailedException();

        var user = _userRepository.GetUser(userRequest.Email);

        return new AuthenticateUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = GenerateJwtToken(user)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        var token = new JwtSecurityToken(null, null,
                                        claims,
                                        expires: DateTime.Now.AddMinutes(20),
                                        signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}