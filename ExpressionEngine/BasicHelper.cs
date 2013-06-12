﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MetraTech.ExpressionEngine
{
    public static class BasicHelper
    {
        private const string NameRegexString = "[a-zA-Z][a-zA-Z0-9_]*";
        private static readonly Regex NameRegex;
        private static readonly Regex NamespaceRegex;
        private static readonly Regex FullNameRegex;

        #region Constructor
        static BasicHelper()
        {
            //Create the Name regex
            NameRegex = new Regex(WrapToMatchAll(NameRegexString));

            //Create the base dotted name patten which will be varied slightly for Namespace and full name
            var dottedNamePattern = string.Format(CultureInfo.InvariantCulture, @"{0}(\.{0})", NameRegexString);
            NamespaceRegex = new Regex(WrapToMatchAll(dottedNamePattern + "*"));
            FullNameRegex  = new Regex(WrapToMatchAll(dottedNamePattern + "+"));
        }
        #endregion

        #region Name Methods
        private static string WrapToMatchAll(string regex)
        {
            return "^" + regex + "$";
        }
        public static bool NameIsValid(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            return NameRegex.IsMatch(name);
        }
        public static bool FullNameIsValid(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return false;
            return FullNameRegex.IsMatch(fullName);
        }
        public static bool NamespaceIsValid(string _namespace)
        {
            if (string.IsNullOrEmpty(_namespace))
                return false;
            return NamespaceRegex.IsMatch(_namespace);
        }

        public static string GetNameFromFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return null;

            var parts = fullName.Split('.');
            return parts[parts.Length - 1];
        }

        public static string GetNamespaceFromFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return null;

            var parts = fullName.Split('.');
            if (parts.Length == 1)
                return null;
            return fullName.Substring(0, fullName.Length - parts[parts.Length - 1].Length - 1);
        }

        public static string GetFullName(string _namespace, string name)
        {
            return _namespace + "." + name;
        }
        #endregion

        #region Methods
        public static string CleanUpWhiteSpace(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            value = value.Trim();
            value = Regex.Replace(value, "[\t\r\n]", "");
            value = Regex.Replace(value, "[ ]+", " ");
            return value;
        }

        //
        //Deterimines if the parameter is even
        //
        public static bool IsEven(int number)
        {
            return (number % 2 == 0);
        }


        //
        //Returns a random string of the specified length
        //
        public static string GetRandomString(Random random, int min, int max, bool lowercase)
        {
            if (random == null)
                throw new ArgumentNullException("random");

            var size = random.Next(min, max);

            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            if (lowercase)
                return builder.ToString().ToLower(CultureInfo.InvariantCulture);
            return builder.ToString();
        }
        #endregion

    }
}
