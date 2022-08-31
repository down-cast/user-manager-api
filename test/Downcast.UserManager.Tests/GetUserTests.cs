using System.Net;

using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils;

using Refit;

namespace Downcast.UserManager.Tests;

public class GetUserTests : BaseTestClass
{
    [Fact]
    public async Task GetUserSuccess()
    {
        ApiResponse<User> response = await Client.GetUser(ExistingUser.Id).ConfigureAwait(false);
        response.IsSuccessStatusCode.Should().BeTrue();
        ExistingUser.Should().BeEquivalentTo(response.Content);
    }

    [Fact]
    public async Task GetUserNotFound()
    {
        ApiResponse<User> response = await Client.GetUser(ExistingUser.Id + "1").ConfigureAwait(false);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}