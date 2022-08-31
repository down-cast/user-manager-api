using System.Net;

using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils;
using Downcast.UserManager.Tests.Utils.DataFakers;

using Refit;

namespace Downcast.UserManager.Tests;

public class UpdateUserTests : BaseTestClass
{
    [Fact]
    public async Task UpdateUser_Success()
    {
        UpdateUserInputModel updateUserInput = new UpdateUserInputModelFaker().Generate();

        HttpResponseMessage response = await Client.UpdateUser(ExistingUser.Id, updateUserInput).ConfigureAwait(false);
        response.IsSuccessStatusCode.Should().BeTrue();

        ApiResponse<User> updatedUser = await Client.GetUser(ExistingUser.Id).ConfigureAwait(false);
        updatedUser.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedUser.Content!.DisplayName.Should().Be(updateUserInput.DisplayName);
        updatedUser.Content!.Description.Should().Be(updateUserInput.Description);
        updatedUser.Content!.ProfilePictureUri.Should().Be(updateUserInput.ProfilePictureUri);
        updatedUser.Content!.EmailValidated.Should().Be(updateUserInput.EmailValidated ?? false);
        updatedUser.Content!.SocialLinks.Should().BeEquivalentTo(updateUserInput.SocialLinks);
    }
}