using System.Security.Cryptography;
using System.Text;

namespace CliniaApi.Services;

public static class PasswordHasher
{
    public static string HashToHex(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }

    public static bool Verify(string input, string expectedHash)
    {
        if (string.IsNullOrEmpty(expectedHash))
        {
            return false;
        }

        var hash = HashToHex(input);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(hash),
            Encoding.ASCII.GetBytes(expectedHash));
    }
}
