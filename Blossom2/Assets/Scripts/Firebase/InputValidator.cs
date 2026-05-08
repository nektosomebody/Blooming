using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class InputValidator
{
    private static readonly Regex NamePattern    = new Regex(@"^[А-Яа-яёЁA-Za-z]+$");
    private static readonly Regex EmailPattern   = new Regex(@"^[A-Za-z@._]+$");
    private static readonly Regex PasswordPattern = new Regex(@"^[A-Za-z0-9@._]+$");

    // Returns null if valid, otherwise — error message.
    public static string ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length < 3 || name.Length > 20)
            return "Имя: от 3 до 20 символов";
        if (!NamePattern.IsMatch(name))
            return "Только буквы русского и английского алфавита";
        return null;
    }

    public static string ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || email.Length < 3 || email.Length > 50)
            return "Почта: от 3 до 50 символов";
        if (!EmailPattern.IsMatch(email))
            return "Только латинские буквы и символы @, ., _";
        return null;
    }

    public static string ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 3 || password.Length > 50)
            return "Пароль: от 3 до 50 символов";
        if (!PasswordPattern.IsMatch(password))
            return "Только латинские буквы, цифры и символы @, ., _";

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
            return "Не менее 4 цифр";
        if (distinctLetters.Count < 5)
            return "Не менее 5 различных букв";

        return null;
    }
}
