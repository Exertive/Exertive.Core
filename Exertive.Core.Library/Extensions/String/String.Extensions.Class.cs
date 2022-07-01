
namespace Exertive.Core.Extensions
{

    #region Dependencies

    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    #endregion Dependencies

    public static class StringExtensions
    {

        #region Private Static Fields

        private static readonly string WordPattern = "((\\w+)[,.;:\'\"-+=\\/ ]*)";

        private static readonly Regex WordExpression = new(WordPattern, RegexOptions.Compiled);

        #endregion Private Static Fields

        #region Public Static Extension Methods

        /// <summary>
        /// Converts the input string provided to Camel Case.
        /// </summary>
        /// <param name="input">The Input string to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToCamelCase(this string input)
        {
            var start = -1;
            if (!String.IsNullOrWhiteSpace(input))
            {
                start = input.TakeWhile((character) => !Char.IsLetter(character)).Count();
            }
            return (start < 0) ? input : String.Concat(input[..start], input[start..start].ToLowerInvariant(), input[(start + 1)..]);
        }


        /// <summary>
        /// Converts the input string provided to Initial Capitals.
        /// </summary>
        /// <param name="input">The Input string to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToInitialCaps(this string input)
        {
            if (!String.IsNullOrWhiteSpace(input))
            {
                var matches = StringExtensions.WordExpression.Matches(input);
                var output = "";
                foreach (Match match in matches)
                {
                    var term = match.Groups[0].Value;
                    output += term[..1].ToUpper() + term[1..].ToLower();
                }
                return output;
            }
            return input;
        }

        #endregion Public Static Extension Methods
    }
}
