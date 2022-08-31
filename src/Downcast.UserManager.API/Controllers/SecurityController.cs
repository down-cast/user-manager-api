using Downcast.UserManager.Model.Input;
using Downcast.UserManager.Security;

using Microsoft.AspNetCore.Mvc;

namespace Downcast.UserManager.API.Controllers;

[ApiController]
[Route("api/v1/users")]
public class SecurityController : ControllerBase
{
    private readonly ISecurityManager _securityManager;

    public SecurityController(ISecurityManager securityManager)
    {
        _securityManager = securityManager;
    }


    /// <summary>
    /// Validates credentials of a user given its email and password
    /// </summary>
    /// <param name="authRequest"></param>
    /// <returns>Jwt token with its expiration date</returns>
    [HttpPost("validate-credentials")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> ValidateCredentials([FromBody] AuthenticationRequest authRequest)
    {
        bool credentialsValid = await _securityManager.ValidateCredentials(authRequest).ConfigureAwait(false);
        if (credentialsValid)
        {
            return Ok();
        }

        return Unauthorized();
    }


    /// <summary>
    /// Update the user's password
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="passwordInput"></param>
    /// <returns></returns>
    [HttpPut("{userId}/password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task UpdatePassword(string userId, UpdatePasswordInput passwordInput)
    {
        return _securityManager.UpdatePassword(userId, passwordInput);
    }
}