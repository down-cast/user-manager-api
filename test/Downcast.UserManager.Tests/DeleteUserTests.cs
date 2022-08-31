using System.Net;

using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils;
using Downcast.UserManager.Tests.Utils.DataFakers;

using Refit;

namespace Downcast.UserManager.Tests;

public class DeleteUserTests : BaseTestClass
{
    [Fact]
    public async Task DeleteUser_Success()
    {
        // Ensure user exists
        ApiResponse<User> result = await Client.GetUser(ExistingUser.Id).ConfigureAwait(false);
        result.IsSuccessStatusCode.Should().BeTrue();
        ExistingUser.Should().BeEquivalentTo(result.Content);

        // Delete user
        HttpResponseMessage response = await Client.DeleteUser(ExistingUser.Id).ConfigureAwait(false);
        response.IsSuccessStatusCode.Should().BeTrue();

        result = await Client.GetUser(ExistingUser.Id).ConfigureAwait(false);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_NotFound()
    {
        // Delete user
        HttpResponseMessage response = await Client.DeleteUser(ExistingUser.Id + "1").ConfigureAwait(false);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}