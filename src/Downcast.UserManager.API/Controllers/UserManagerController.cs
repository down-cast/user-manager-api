using Microsoft.AspNetCore.Mvc;

namespace Downcast.UserManager.API.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserManagerController : ControllerBase
{
    private readonly ILogger<UserManagerController> _logger;

    public UserManagerController(ILogger<UserManagerController> logger)
    {
        _logger = logger;
    }
}