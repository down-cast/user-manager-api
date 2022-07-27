using Downcast.SessionManager.SDK.Client;
using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Dtos;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Downcast.UserManager.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/users")]
public class UserManagerController : ControllerBase
{
    private readonly ILogger<UserManagerController> _logger;
    private readonly IUserManager _userManager;
    private readonly ISessionManagerClient _client;
    public UserManagerController(ILogger<UserManagerController> logger, IUserManager userService, ISessionManagerClient client)
    {
        _logger = logger;
        _userManager = userService;
        _client = client;
    }

    [AllowAnonymous]
    [HttpGet("{userId}")]
    public async Task<UserDto> GetUserById(string userId)
    {
         return await _userManager.GetUserById(userId);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> CreateUser(CreateUser userInfo)
    {
        string documentId = await _userManager.CreateUser(userInfo);
        CreatedAtActionResult x = CreatedAtAction(nameof(GetUserById), new { userId = documentId }, null);
        return x;
    }

    [AllowAnonymous]
    [HttpDelete("{userId}")]
    public Task DeleteUser(string userId)
    {
        return _userManager.DeleteUserById(userId);
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task UpdateUser(UserDto userInfo)
    {
        await _userManager.UpdateUser(userInfo);
    }

    [AllowAnonymous]
    [HttpPost("Authenticate")]
    public async Task<string>  AuthenticateUser(AuthenticateUser credentials)
    {
        return await _userManager.AuthenticateUser(credentials);
    }

    [HttpPost("Test")]
    public async Task<IActionResult> Test()
    {
        string? accessToken = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if(accessToken is null)
        {
            return Unauthorized();  
        }
        IDictionary<string, object>? userClaims = await _client.ValidateSessionToken(accessToken);
        return Ok(userClaims);
    }
}