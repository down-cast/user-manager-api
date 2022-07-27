using Downcast.UserManager.Model;
using Downcast.UserManager.Model.Dtos;
using Downcast.UserManager.Repository.Domain;

using Google.Api.Gax;
using Google.Cloud.Firestore;

using Mapster;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Downcast.UserManager.Repository.Config
{
    public static class UserManagerRepositoryExtensions
    {
        public static WebApplicationBuilder AddFirestoreRepositoryConfigurations(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions<FirestoreRepositoryOptions>()
            .Bind(builder.Configuration.GetSection(FirestoreRepositoryOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
            builder.Services.AddScoped<IUserRepositoryAPI, UserRepositoryAPI>();
            TypeAdapterConfig<CreateUser, User>.NewConfig();
            TypeAdapterConfig<User, UserDto>.NewConfig();
            TypeAdapterConfig<PasswordManager.PasswordInfo, PasswordInfo>.NewConfig();
            TypeAdapterConfig<PasswordInfo, PasswordManager.PasswordInfo >.NewConfig();

            builder.Services.AddSingleton(provider =>
            {
                IOptions<FirestoreRepositoryOptions> repositoryOptions = provider.GetRequiredService<IOptions<FirestoreRepositoryOptions>>();
                return new FirestoreDbBuilder
                {
                    ProjectId = repositoryOptions.Value.ProjectID,
                    EmulatorDetection = EmulatorDetection.EmulatorOrProduction
                }.Build();
            });
            builder.Services.AddScoped<IUserRepository, UserFirestoreRepository>();
            return builder;
        }
    }
}
