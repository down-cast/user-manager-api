using System.Net;

using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils;
using Downcast.UserManager.Tests.Utils.DataFakers;

using Refit;

namespace Downcast.UserManager.Tests;

public class GetByEmailTests : BaseTestClass
{
    [Fact]
    public async Task GetByEmail_Success()
    {
        ApiResponse<User> result = await Client.GetByEmail(ExistingUser.Email).ConfigureAwait(false);
        result.IsSuccessStatusCode.Should().BeTrue();
        result.Content!.Should().BeEquivalentTo(ExistingUser);
    }

    [Fact]
    public async Task GetByEmail_NotFound()
    {
        ApiResponse<User> result = await Client.GetByEmail(ExistingUser.Email + "1").ConfigureAwait(false);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}