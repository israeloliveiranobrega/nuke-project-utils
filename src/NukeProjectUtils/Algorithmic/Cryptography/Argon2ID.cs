/*
Conhecer os outros é inteligência, conhecer-se a si próprio é verdadeira sabedoria
*/

using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;
using NukeProjectUtils.DataStructure.OptionsObjects;

namespace NukeProjectUtils.Algorithmic.Cryptography;

/// <summary>
/// Provides cryptographic services for password hashing and verification using the Argon2id key derivation function.
/// </summary>
public static class Argon2ID
{
    private static Argon2IDOptions Options = new();

    /// <summary>
    /// Applies the specified application configuration settings to the cryptographic provider.
    /// </summary>
    /// <param name="options">The configuration options containing parameters like memory cost, time cost, and parallelism.</param>
    public static void Configure(IOptions<Argon2IDOptions> options)
    {
        Options = options.Value;
    }

    /// <summary>
    /// Generates a secure string representation of the hashed credential incorporating the salt and algorithmic parameters.
    /// </summary>
    /// <param name="rawPassword">The plaintext credential provided by the user.</param>
    /// <returns>A formatted string containing the cryptographic parameters, encoded salt, and encoded hash.</returns>
    public static string Encrypt(string rawPassword)
    {
        byte[] saltGenerated = GenerateSalt();
        byte[] hashGenerated = GenerateHash(rawPassword + Options.Pepper, saltGenerated);

        string convertedSalt = Convert.ToBase64String(saltGenerated);
        string convertedHash = Convert.ToBase64String(hashGenerated);

        string result = $"$argon2id" +
            $"$m={Options.MemoryCost}" +
            $"$t={Options.TimeCost}" +
            $"$p={Options.Parallelism}" +
            $"${EncodeUrlSafe(convertedSalt)}$" +
            $"{EncodeUrlSafe(convertedHash)}";

        return result;
    }

    /// <summary>
    /// Validates a plaintext credential against a previously generated Argon2id hash string.
    /// </summary>
    /// <param name="rawPassword">The plaintext credential to evaluate.</param>
    /// <param name="hashPassword">The stored string containing the hash, salt, and configuration parameters.</param>
    /// <returns>True if the computed hash matches the stored hash; otherwise, false.</returns>
    public static bool Verify(string rawPassword, string hashPassword)
    {
        Argon2IDHashSplit splitHash = SplitHash(hashPassword);

        byte[] hashBytes;

        using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(rawPassword + Options.Pepper)))
        {
            string decodedSalt = DecodeUrlSafe(splitHash.Salt);

            argon2.Salt = Convert.FromBase64String(decodedSalt);
            argon2.DegreeOfParallelism = splitHash.Parallelism;
            argon2.Iterations = splitHash.TimeCost;
            argon2.MemorySize = splitHash.MemoryCost;

            hashBytes = argon2.GetBytes(Options.HashSize);
        }

        string decodedHash = DecodeUrlSafe(splitHash.Hash);

        bool result = CryptographicOperations.FixedTimeEquals(hashBytes, Convert.FromBase64String(decodedHash));

        return result;
    }

    #region Generators

    /// <summary>
    /// Produces a cryptographically secure random sequence of bytes to be used as a salt.
    /// </summary>
    /// <returns>An array of random bytes.</returns>
    private static byte[] GenerateSalt()
    {
        byte[] saltGenerated = new byte[Options.SaltSize];
        RandomNumberGenerator.Fill(saltGenerated);
        return saltGenerated;
    }

    /// <summary>
    /// Computes the Argon2id hash using the specified plaintext, salt, and configured algorithmic costs.
    /// </summary>
    /// <param name="rawPassword">The plaintext credential.</param>
    /// <param name="salt">The cryptographic salt.</param>
    /// <returns>The computed hash as a byte array.</returns>
    private static byte[] GenerateHash(string rawPassword, byte[] salt)
    {
        byte[] hashGenerated;

        using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(rawPassword)))
        {
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = Options.Parallelism;
            argon2.Iterations = Options.TimeCost;
            argon2.MemorySize = Options.MemoryCost;

            hashGenerated = argon2.GetBytes(Options.HashSize);
        }

        return hashGenerated;
    }

    /// <summary>
    /// Parses the formatted hash string to extract the cryptographic parameters, salt, and hash components.
    /// </summary>
    /// <param name="hashPassword">The fully formatted Argon2id string.</param>
    /// <returns>An object containing the parsed algorithmic variables.</returns>
    /// <exception cref="System.FormatException">Thrown when the numeric parameters in the hash string are not valid integers.</exception>
    /// <exception cref="System.IndexOutOfRangeException">Thrown when the hash string does not contain the expected number of delimited segments.</exception>
    private static Argon2IDHashSplit SplitHash(string hashPassword)
    {
        string[] slices = hashPassword.Split('$');

        var argonOptions = new Argon2IDHashSplit()
        {
            MemoryCost = int.Parse(slices[2].Split('=')[1]),
            TimeCost = int.Parse(slices[3].Split('=')[1]),
            Parallelism = int.Parse(slices[4].Split('=')[1]),
            Salt = slices[5],
            Hash = slices[6]
        };

        return argonOptions;
    }

    #endregion

    #region Private Tools

    /// <summary>
    /// Replaces Base64 characters that are unsafe for URL transmission with safe alternatives.
    /// </summary>
    /// <param name="base64Hash">The standard Base64 encoded string.</param>
    /// <returns>A URL-safe encoded string.</returns>
    private static string EncodeUrlSafe(string base64Hash)
    {
        return base64Hash.Replace("+", "#").Replace("/", "-");
    }

    /// <summary>
    /// Restores standard Base64 characters from a URL-safe encoded string.
    /// </summary>
    /// <param name="base64Hash">The URL-safe encoded string.</param>
    /// <returns>The standard Base64 encoded string.</returns>
    private static string DecodeUrlSafe(string base64Hash)
    {
        return base64Hash.Replace("#", "+").Replace("-", "/");
    }

    #endregion
}