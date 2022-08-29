using AutoMapper;

using Downcast.UserManager.Model;

using PasswordInfo = Downcast.UserManager.Repository.Domain.PasswordInfo;
using User = Downcast.UserManager.Repository.Domain.User;

namespace Downcast.UserManager.Repository.Config;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Allow to map public and internal properties
        AllowNullCollections = true;
        AllowNullDestinationValues = true;
        ShouldMapProperty = arg => arg?.GetMethod?.IsPublic is true || arg?.GetMethod?.IsAssembly is true;

        CreateMap<CreateUser, User>();
        CreateMap<User, Model.User>();

        CreateMap<PasswordInfo, Model.PasswordInfo>();
        CreateMap<Model.PasswordInfo, PasswordInfo>();

        CreateMap<SocialLinks, Domain.SocialLinks>();
        CreateMap<Domain.SocialLinks, SocialLinks>();
    }
}