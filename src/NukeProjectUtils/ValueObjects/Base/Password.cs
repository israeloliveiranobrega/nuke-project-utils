using System.Text.RegularExpressions;

namespace NukeProjectUtils.ValueObjects.Base;

public record Password
{
    public string Hash { get; init; }

    private Password() { }

    public Password(string password)
    {
        ValidPassword(password);

        Hash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool Validate(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private static void ValidPassword(string password)
    {
        string regexPattern = @"^(?=.*[A-Z])(?=.*\d).{15,}$";

        if (!Regex.IsMatch(password, regexPattern))
            throw new ArgumentException("A senha não atende os requesitos.");
    }
}
