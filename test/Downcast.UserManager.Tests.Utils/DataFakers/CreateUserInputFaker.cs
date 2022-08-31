using Bogus;

using Downcast.UserManager.Client.Model;

namespace Downcast.UserManager.Tests.Utils.DataFakers;

public sealed class CreateUserInputFaker : Faker<CreateUserInputModel>
{
    public CreateUserInputFaker()
    {
        RuleFor(u => u.DisplayName, faker => faker.Name.FullName());
        RuleFor(u => u.Email, faker => faker.Internet.Email().ToLower());
        RuleFor(u => u.Password, faker => faker.Internet.Password());
    }
}