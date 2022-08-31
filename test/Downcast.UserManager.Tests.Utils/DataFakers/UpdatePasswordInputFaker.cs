using Bogus;

using Downcast.UserManager.Client.Model;

namespace Downcast.UserManager.Tests.Utils.DataFakers;

public sealed class UpdatePasswordInputFaker : Faker<UpdatePasswordInput>
{
    public UpdatePasswordInputFaker()
    {
        RuleFor(u => u.Password, faker => faker.Internet.Password());
    }
}