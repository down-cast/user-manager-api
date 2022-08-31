using Downcast.UserManager.API.Controllers;
using Downcast.UserManager.Repository.Internal;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Downcast.UserManager.Tests.Utils;

public class UserManagerServerInstance : WebApplicationFactory<UsersController>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            services.Configure<UserRepositoryOptions>(options =>
            {
                options.EmailsCollection = "emails_test";
                options.UsersCollection = "users_test";
                options.ProjectId = ProjectId;
            });
        });
    }

    private static string ProjectId => Environment.GetEnvironmentVariable("FIRESTORE_PROJECT_ID") ??
                                       throw new InvalidOperationException(
                                           "FIRESTORE_PROJECT_ID environment variable is not set");
}