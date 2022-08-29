using Downcast.UserManager.Repository.Internal;

using Google.Api.Gax;
using Google.Cloud.Firestore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Downcast.UserManager.Repository.Config;

public static class RepositoryConfigExtensions
{
    public static IServiceCollection AddUserRepository(this IServiceCollection services, IConfiguration configuration)
    {
        AddMapper(services);
        AddFirestoreDb(services);

        services
            .AddOptions<UserRepositoryOptions>()
            .Bind(configuration.GetSection(UserRepositoryOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserRepositoryInternal, UserRepositoryInternal>();

        return services;
    }

    private static void AddMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
    }

    private static void AddFirestoreDb(IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            IOptions<UserRepositoryOptions> options = provider.GetRequiredService<IOptions<UserRepositoryOptions>>();
            return new FirestoreDbBuilder
            {
                ProjectId = options.Value.ProjectId,
                EmulatorDetection = EmulatorDetection.EmulatorOrProduction
            }.Build();
        });
    }
}