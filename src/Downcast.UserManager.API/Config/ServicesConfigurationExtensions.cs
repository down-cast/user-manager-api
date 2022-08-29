using Downcast.UserManager.Authentication;
using Downcast.UserManager.Cryptography;
using Downcast.UserManager.Repository.Config;

namespace Downcast.UserManager.API.Config;

public static class ServicesConfigurationExtensions
{
    public static WebApplicationBuilder AddUserManagerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddUserRepository(builder.Configuration);
        AddAuthenticationManager(builder);
        AddUserManager(builder);
        AddPasswordManager(builder);
        return builder;
    }

    private static void AddPasswordManager(WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<PasswordManagerOptions>()
            .Bind(builder.Configuration.GetSection(PasswordManagerOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddSingleton<IPasswordManager, PasswordManager>();
    }

    private static void AddUserManager(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IUserManager, UserManager>();
    }

    private static void AddAuthenticationManager(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IAuthenticationManager, AuthenticationManager>();
    }
}