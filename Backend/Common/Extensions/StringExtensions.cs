using System;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string str, string value)
        {
            return string.IsNullOrWhiteSpace(value) || str.ToLowerInvariant().Contains(value.ToLowerInvariant());
        }

        public static bool IsValue(this string str, string value)
        {
            return string.Equals(str, value, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}