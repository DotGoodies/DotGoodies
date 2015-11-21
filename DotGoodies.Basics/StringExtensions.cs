using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DotGoodies.Basics
{
    public static class StringExtensions
    {
        public static string JoinStrings(this IEnumerable<string> values, string separator,
            string replaceNullsWith = null)
        {
            return values == null 
                ? string.Empty 
                : string.Join(separator, values.Select(v => v ?? replaceNullsWith));
        }

        public static string FormatWith(this string template, params object[] values)
        {
            return string.Format(CultureInfo.InvariantCulture, template, values);
        }
    }
}