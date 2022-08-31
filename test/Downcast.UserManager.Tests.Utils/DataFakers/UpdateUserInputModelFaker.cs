using Bogus;

using Downcast.UserManager.Client.Model;

namespace Downcast.UserManager.Tests.Utils.DataFakers;

public sealed class UpdateUserInputModelFaker : Faker<UpdateUserInputModel>
{
    public UpdateUserInputModelFaker()
    {
        RuleFor(u => u.DisplayName, faker => faker.Name.FullName());
        RuleFor(u => u.Description, faker => faker.Lorem.Paragraph());
        RuleFor(u => u.EmailValidated, faker => faker.Random.Bool());
        RuleFor(u => u.ProfilePictureUri, faker => faker.Internet.UrlWithPath());
        RuleFor(u => u.SocialLinks, faker => new SocialLinks()
        {
            Facebook = faker.Internet.UrlWithPath(),
            GitHub = faker.Internet.UrlWithPath(),
            LinkedIn = faker.Internet.UrlWithPath(),
            StackOverflow = faker.Internet.UrlWithPath(),
            Twitter = faker.Internet.UrlWithPath(),
        });
    }
}