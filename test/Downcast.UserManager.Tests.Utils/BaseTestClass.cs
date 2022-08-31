using Downcast.UserManager.Client;
using Downcast.UserManager.Client.Model;
using Downcast.UserManager.Tests.Utils.DataFakers;

using Refit;

using Xunit;

namespace Downcast.UserManager.Tests.Utils;

public class BaseTestClass : IAsyncLifetime
{
    protected User ExistingUser { get; private set; } = null!;
    protected string ExistingUserPassword { get; private set; } = null!;

    private readonly UserManagerServerInstance _server = new();
    public IUserManagerClient Client { get; }

    public BaseTestClass()
    {
        HttpClient httpClient = _server.CreateClient();
        Client = RestService.For<IUserManagerClient>(httpClient);
    }

    public async Task InitializeAsync()
    {
        CreateUserInputModel createUser = new CreateUserInputFaker().Generate();
        ApiResponse<User> result = await Client.CreateUser(createUser).ConfigureAwait(false);
        ExistingUser = result.Content!;
        ExistingUserPassword = createUser.Password;
    }

    public async Task DisposeAsync()
    {
        await Client.DeleteUser(ExistingUser.Id).ConfigureAwait(false);
        await _server.DisposeAsync().ConfigureAwait(false);
    }
}