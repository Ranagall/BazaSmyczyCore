using System;

namespace BazaSmyczy.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string ToTitleCase(this string value)
        {
            var tokens = value.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1);
            }

            return string.Join(" ", tokens);
        }
    }
}
