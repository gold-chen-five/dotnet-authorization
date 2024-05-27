
namespace Util;
public static class Rand
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
    private static readonly Random RandomGenerator = new();

    public static string RandomString(int n)
    {
        if (n <= 0) throw new ArgumentException("Length must be greater than 0.", nameof(n));

        char[] result = new char[n];
        int alphabetLength = Alphabet.Length;

        for (int i = 0; i < n; i++)
        {
            int randomIndex = RandomGenerator.Next(alphabetLength);
            result[i] = Alphabet[randomIndex];
        }

        return new string(result);
    }

    public static string RandomUser()
    {
        return RandomString(6);
    }
}