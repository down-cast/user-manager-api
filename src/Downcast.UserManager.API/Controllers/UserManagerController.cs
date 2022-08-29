using System.ComponentModel.DataAnnotations;

using Downcast.UserManager.Model;

using Microsoft.AspNetCore.Mvc;

using CreateUserInputModel = Downcast.UserManager.Model.Input.CreateUserInputModel;

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
    /// Retrieves user by email
    /// </summary>
    /// <returns>The user info</returns>
    [HttpGet("email/{email}")]
    public Task<User> GetByEmail([Required, EmailAddress] string email)
    {
        return _userManager.GetUserByEmail(email);
    }


    /// <summary>
    /// Deletes a user given the userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpDelete("{userId}")]
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


    /// <summary>
    /// Updates a user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="updateUser"></param>
    /// <returns></returns>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task UpdateUser(string userId, UpdateUserInputModel updateUser)
    {
        return _userManager.UpdateUser(userId, updateUser);
    }


    /// <summary>
    /// Adds a list of roles to a user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    [HttpPut("{userId}/roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task AddRoles(string userId, [FromBody] string[] roles)
    {
        return _userManager.AddRoles(userId, roles);
    }

    /// <summary>
    /// Removes a list of roles from a user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    [HttpDelete("{userId}/roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task RemoveRoles(string userId, [FromBody] string[] roles)
    {
        return _userManager.RemoveRoles(userId, roles);
    }
}