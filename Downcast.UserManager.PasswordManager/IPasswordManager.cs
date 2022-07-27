namespace Downcast.UserManager.PasswordManager
{
    public interface IPasswordManager
    {
        PasswordInfo HashPassword(string password);
        (bool verified, bool upgradeNeeded) CheckPasswordHash(
            string password,
            PasswordInfo passwordInfo);
    }
}
