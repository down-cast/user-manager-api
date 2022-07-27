namespace Downcast.UserManager.PasswordManager
{
    public class PasswordInfo
    {
        public string PasswordHash { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public int Iterations { get; set; } 
    }
}
