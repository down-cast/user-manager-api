namespace Downcast.UserManager.Cryptography;

public class PasswordManagerOptions
{
    public int Iterations { get; set; } = 30000;
    public int SaltSizeInBytes { get; set; } = 64;
    public int HashSizeInBytes { get; set; } = 512;
}