using System.Net;

using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils;
using Downcast.UserManager.Tests.Utils.DataFakers;

using Refit;

namespace Downcast.UserManager.Tests;

public class CreateUserTests : BaseTestClass
{
    [Fact]
    public async Task CreateUser_Success()
    {
        CreateUserInputModel createUser = new CreateUserInputFaker().Generate();
        ApiResponse<User> result = await Client.CreateUser(createUser).ConfigureAwait(false);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Content.Should().NotBeNull();
        result.Content!.Email.Should().Be(createUser.Email);
        result.Content!.DisplayName.Should().Be(createUser.DisplayName);
    }
    
    [Fact]
    public async Task CreateUser_EmailAlreadyTaken()
    {
        CreateUserInputModel createUser = new CreateUserInputFaker().Generate();
        ApiResponse<User> _ = await Client.CreateUser(createUser).ConfigureAwait(false);
        ApiResponse<User> result = await Client.CreateUser(createUser).ConfigureAwait(false);
        result.StatusCode.Should().Be(HttpStatusCode.Conflict);
        result.Content.Should().BeNull();
    }
}