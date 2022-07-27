namespace Downcast.UserManager.PasswordManager
{
    public class HashingOptions
    {
        public const string SectionName = "HashingSettings";
        public int Iterations { get; set; }
    }
}
