using System.Text;

namespace EManagementVSA.Features.Employee.Create;

public class CreateEmployeeHelper
{
    public static string GenerateInitialPassword(int length = 8)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        var stringBuilder = new StringBuilder();
        var random = new Random();

        // Ensure the password contains at least one digit, one uppercase letter, and one lowercase letter
        stringBuilder.Append(GetRandomChar(random, "0123456789")); // Add a random digit
        stringBuilder.Append(GetRandomChar(random, "abcdefghijklmnopqrstuvwxyz")); // Add a random lowercase letter
        stringBuilder.Append(GetRandomChar(random, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")); // Add a random uppercase letter
        stringBuilder.Append(GetRandomChar(random, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")); // Add a random uppercase letter

        // Fill the remaining characters of the password with random characters from validChars
        for (int i = 3; i < length; i++)
        {
            stringBuilder.Append(validChars[random.Next(validChars.Length)]);
        }

        // Shuffle the characters in the password
        var chars = stringBuilder.ToString().ToCharArray();
        for (int i = 0; i < length - 1; i++)
        {
            int j = random.Next(i, length);
            char temp = chars[i];
            chars[i] = chars[j];
            chars[j] = temp;
        }

        return new string(chars);
    }

    private static char GetRandomChar(Random random, string validChars)
    {
        return validChars[random.Next(validChars.Length)];
    }
}