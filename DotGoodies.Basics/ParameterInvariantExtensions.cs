using System;
using System.Text.RegularExpressions;

namespace DotGoodies.Basics
{
    public static class ParameterInvariantExtensions
    {
        public static T NotNull<T>(this T @object, string parameterName)
        {
            if (@object is ValueType)
            {
                return @object;
            }

            if (Equals(@object, default(T)))
            {
                throw new ArgumentNullException(parameterName);
            }

            return @object;
        }

        public static string NotNullOrWhitespace(this string text, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("A meaningful string expected.", parameterName);
            }

            return text;
        }

        public static T NotDefault<T>(this T value, string parameterName)
        {
            if (Equals(default(T), value))
            {
                throw new ArgumentException("Non-default value expected.", parameterName);
            }

            return value;
        }

        public static DateTime IsUtc(this DateTime time, string parameterName)
        {
            if (time.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("UTC time expected.", parameterName);
            }

            return time;
        }

        public static TimeSpan GreaterThenZero(this TimeSpan timeSpan, string parameterName)
        {
            if (timeSpan <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(parameterName, "time span expected to have positive duration");
            }

            return timeSpan;
        }

        public static int GreaterThen(this int value, int lowerBorder, string parameterName)
        {
            if (value <= lowerBorder)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Value greater then {0} was expected.".FormatWith(lowerBorder));
            }

            return value;
        }

        public static T[] NotNullOrEmpty<T>(this T[] array, string parameterName)
        {
            if (array == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (array.Length == 0)
            {
                throw new ArgumentException("Not empty array expected.", parameterName);
            }

            return array;
        }

        public static string Matches(this string value, string regex, string parameterName)
        {
            if (!Regex.IsMatch(value, regex))
            {
                throw new ArgumentException(
                    "Given value '{0}' doesn't match pattern '{1}'".FormatWith(value, regex), 
                    parameterName);
            }

            return value;
        }
    }
}