using Downcast.SessionManager.SDK.Authentication.Extensions;
using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Input;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Downcast.UserManager.API.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserManagerController : ControllerBase
{
    private readonly IUserManager _userManager;

    public UserManagerController(IUserManager userManager)
    {
        _userManager = userManager;
    }


    /// <summary>
    /// Retrieves a user by id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public Task<User> GetUser(string userId)
    {
        return _userManager.GetUser(userId);
    }

    /// <summary>
    /// Retrieves the logged in user
    /// </summary>
    /// <returns>The user info</returns>
    [HttpGet("me")]
    [Authorize]
    public Task<User> GetSelf()
    {
        return _userManager.GetUser(HttpContext.User.UserId());
    }


    /// <summary>
    /// Deletes a user given the userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId}")]
    [Authorize(Roles = RoleNames.Admin)]
    public Task DeleteUser(string userId)
    {
        return _userManager.DeleteUser(userId);
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="userInputModel"></param>
    /// <returns>The newly created user</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<User>> CreateUser(CreateUserInputModel userInputModel)
    {
        User createdUser = await _userManager.CreateUser(userInputModel).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetUser), new { userId = createdUser.Id }, createdUser);
    }
}