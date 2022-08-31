using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils;
using Downcast.UserManager.Tests.Utils.DataFakers;

using Refit;

namespace Downcast.UserManager.Tests;

public class AddRoleTests : BaseTestClass
{
    

    [Fact]
    public async Task AddRoles_Success()
    {
        string[] roles = { "admin", "member" };
        HttpResponseMessage result = await Client.AddRoles(ExistingUser.Id, roles).ConfigureAwait(false);
        result.IsSuccessStatusCode.Should().BeTrue();

        ApiResponse<User> updatedUser = await Client.GetUser(ExistingUser.Id).ConfigureAwait(false);
        updatedUser.IsSuccessStatusCode.Should().BeTrue();
        updatedUser.Content!.Roles.Should().BeEquivalentTo(roles);
    }

    [Fact]
    public async Task AddRoles_Should_Not_Duplicate()
    {
        string[] roles = { "admin", "member" };

        // Add roles twice
        HttpResponseMessage result = await Client.AddRoles(ExistingUser.Id, roles).ConfigureAwait(false);
        result.IsSuccessStatusCode.Should().BeTrue();

        result = await Client.AddRoles(ExistingUser.Id, roles).ConfigureAwait(false);
        result.IsSuccessStatusCode.Should().BeTrue();

        ApiResponse<User> updatedUser = await Client.GetUser(ExistingUser.Id).ConfigureAwait(false);
        updatedUser.IsSuccessStatusCode.Should().BeTrue();
        updatedUser.Content!.Roles.Should().BeEquivalentTo(roles);
        updatedUser.Content!.Roles.Count().Should().Be(roles.Length);
    }
}