using System.Net;
using System.Security.Claims;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.Models.AuthenticationModels;
using InterviewHelper.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        [HttpPost("add")]
        public IActionResult AddUser(UserRequest user)
        {
            try
            {
                var response = _userService.AddUser(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("edit")]
        public IActionResult EditUser(UserUpdateRequest user)
        {
            var userSessionEmail = User.FindFirst(ClaimTypes.Email).Value;

            if (userSessionEmail != user.Email)
                return BadRequest("User is not authorized to perform this action");

            try
            {
                _userService.EditUser(user);
                return Ok();
            }
            catch(UserNotFoundException)
            {
                return BadRequest("User not found");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateUserRequest user)
        {
            try
            {
                return Ok(_userService.AuthenticateUser(user));
            }
            catch (AuthenticationFailedException)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
         }
    }
}