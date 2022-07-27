using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Dtos;
using Downcast.UserManager.PasswordManager;

using Mapster;

namespace Downcast.UserManager.API.Config;

public static class ServicesConfigurationExtensions
{
    public static WebApplicationBuilder AddUserManagerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserManager, UserManager>();
        builder.Services.AddScoped<IPasswordManager, PasswordHashing>();

        TypeAdapterConfig<CreateUser, UserDto>.NewConfig();

        return builder;
    }
}