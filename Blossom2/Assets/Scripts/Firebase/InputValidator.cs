using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class InputValidator
{
    private static readonly Regex NamePattern    = new Regex(@"^[А-Яа-яёЁA-Za-z]+$");
    private static readonly Regex EmailPattern   = new Regex(@"^[A-Za-z0-9@._-]+$");
    private static readonly Regex PasswordPattern = new Regex(@"^[A-Za-z0-9@._-]+$");

    // Returns null if valid, otherwise — error message.
    public static string ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length < 3 || name.Length > 20)
            return "Name: 3 to 20 characters";
        if (!NamePattern.IsMatch(name))
            return "Only letters (Russian and English alphabet)";
        return null;
    }

    public static string ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || email.Length < 3 || email.Length > 50)
            return "Email: 3 to 50 characters";
        if (!EmailPattern.IsMatch(email))
            return "Only Latin letters and characters @, ., _";
        return null;
    }

    public static string ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 3 || password.Length > 50)
            return "Password: 3 to 50 characters";
        if (!PasswordPattern.IsMatch(password))
            return "Only Latin letters, digits, and characters @, ., _";

        int digitCount = 0;
        var distinctLetters = new HashSet<char>();
        foreach (char c in password)
        {
            if (c >= '0' && c <= '9')
                digitCount++;
            else if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                distinctLetters.Add(char.ToLowerInvariant(c));
        }

        if (digitCount < 4)
            return "At least 4 digits required";
        if (distinctLetters.Count < 5)
            return "At least 5 different letters required";

        return null;
    }
}
