
namespace ParadosLib
{
    internal static class Extensions
    {
        public static string ExtractAfterDot(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("String cannot be null or empty.", nameof(input));
            }

            int dotIndex = input.IndexOf('.');
            if (dotIndex == -1 || dotIndex == input.Length - 1)
            {
                return input;
            }

            return input.Substring(dotIndex + 1);
        }

        public static string EnsureEndsWithQuote(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return input.EndsWith("\"") ? input : input + "\"";
        }

        public static string EnsureStartsEndsWithQuote(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var changed = input.StartsWith("\"") ? input : "\"" + input;
            return changed.EndsWith("\"") ? changed : changed + "\"";
        }

        public static string ReplaceQuotesEnsureStartEndsWithQuote(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("String cannot be null or empty.", nameof(input));
            }

            if (input.Length < 3) // Se la stringa è troppo corta per avere una sostituzione significativa
            {
                return input.EndsWith("\"") ? input : input + "\"";
            }

            string middlePart = input.Substring(1, input.Length - 2);

            // Sostituisci i doppi apici nel middlePart con apici singoli
            middlePart = middlePart.Replace("\"", "'");

            // Ricomponi la stringa
            return '"' + middlePart + '"';
        }
    }
}
