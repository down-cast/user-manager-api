using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils;

using Refit;

namespace Downcast.UserManager.Tests;

public class RemoveRolesTests : BaseTestClass
{
    private readonly string[] _roles = { "admin", "member", "author" };

    [Fact]
    public async Task RemoveRoles_Success()
    {
        await Client.AddRoles(ExistingUser.Id, _roles).ConfigureAwait(false);
        HttpResponseMessage response = await Client.RemoveRoles(ExistingUser.Id, _roles).ConfigureAwait(false);
        response.IsSuccessStatusCode.Should().BeTrue();

        ApiResponse<User> user = await Client.GetUser(ExistingUser.Id).ConfigureAwait(false);
        user.Content!.Roles.Should().BeEmpty();
    }

    [Fact]
    public async Task RemoveRoles_Idempotent()
    {
        ExistingUser.Roles.Should().BeEmpty();
        HttpResponseMessage response = await Client.RemoveRoles(ExistingUser.Id, _roles).ConfigureAwait(false);
        response.IsSuccessStatusCode.Should().BeTrue();

        response = await Client.RemoveRoles(ExistingUser.Id, _roles).ConfigureAwait(false);
        response.IsSuccessStatusCode.Should().BeTrue();

        ApiResponse<User> user = await Client.GetUser(ExistingUser.Id).ConfigureAwait(false);
        user.Content!.Roles.Should().BeEmpty();
    }

    [Fact]
    public async Task RemoveRoles_RemoveOne_Success()
    {
        await Client.AddRoles(ExistingUser.Id, _roles).ConfigureAwait(false);
        HttpResponseMessage response = await Client.RemoveRoles(ExistingUser.Id, _roles[..1]).ConfigureAwait(false);
        response.IsSuccessStatusCode.Should().BeTrue();

        ApiResponse<User> user = await Client.GetUser(ExistingUser.Id).ConfigureAwait(false);
        user.Content!.Roles.Should().BeEquivalentTo(_roles[1..]);
    }
}