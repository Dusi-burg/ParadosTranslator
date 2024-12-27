using System.Text.RegularExpressions;

namespace ParadosLib
{
    public static class Helper
    {
        public static bool TraductionIsCompatible(string originalValue, string traducedValue)
        {
            List<string> originalSpecial = ExtractStringsBetweenDollars(originalValue);
            List<string> traducedSpecial = ExtractStringsBetweenDollars(traducedValue);

            bool specialAreEquals = AreListsEqual(originalSpecial, traducedSpecial);

            List<string> originalSquare = ExtractStringsBetweenSquare(originalValue);
            List<string> traducedSquare = ExtractStringsBetweenSquare(traducedValue);

            bool squareAreEquals = AreListsEqual(originalSquare, traducedSquare);

            return specialAreEquals && squareAreEquals;
        }

        public static bool ApplySquareAdjustament(string originalValue, string traducedValue, out string changedTraductionValue)
        {
            List<string> compatibileName = new List<string> { "GetName", "GetNameDef", "GetNameDefCap" };
            changedTraductionValue = traducedValue;
            bool adjusted = false;
            int indxChange = 0;
            List<string> originalSquare = ExtractStringsBetweenSquare(originalValue);
            List<Tuple<string, int>> traducedSquare = ExtractStringsAndIndexBetweenSquare(traducedValue);
            for (int i = 0; i < originalSquare.Count; i++)
            {
                if (traducedSquare.Count < i + 1)
                    break;
                ApplyAdjust("FROM.GetNameDefCap", compatibileName, ref changedTraductionValue, ref adjusted, originalSquare[i], traducedSquare[i], ref indxChange);
                ApplyAdjust("FROM.GetNameDef", compatibileName, ref changedTraductionValue, ref adjusted, originalSquare[i], traducedSquare[i], ref indxChange);                
                ApplyAdjust("FROM.GetName", compatibileName, ref changedTraductionValue, ref adjusted, originalSquare[i], traducedSquare[i], ref indxChange);
            }
            return adjusted;
        }

        private static void ApplyAdjust(string originalMatch, List<string> traducedCompatible, ref string changedTraductionValue, ref bool adjusted, string originalSquare, Tuple<string, int> traducedSquare, ref int indxChange)
        {
            if (originalSquare != traducedSquare.Item1 && originalSquare.ExtractAfterDot() == originalMatch.ExtractAfterDot() && traducedCompatible.Contains(traducedSquare.Item1.ExtractAfterDot()))
            {
                int currentLength = changedTraductionValue.Length;
                int indxStart = changedTraductionValue.IndexOf("[", traducedSquare.Item2 - 1 + indxChange);
                int indxStop = changedTraductionValue.IndexOf("]", traducedSquare.Item2 + indxChange);
                string first = changedTraductionValue.Substring(0, indxStart);
                string second = changedTraductionValue.Substring(indxStop + 1);
                changedTraductionValue = first + "[" + originalSquare + "]" + second;
                adjusted = true;
                indxChange += changedTraductionValue.Length - currentLength;
            }
        }

        public static string ApplyUpperCaseOnSquare(string originalValue, out bool changeApplied)
        {
            string result = originalValue;
            changeApplied = false;
            List<string> originalSquare = ExtractStringsBetweenSquare(originalValue);
            foreach (string s in originalSquare)
            {
                int idx = s.IndexOf('.');
                if (idx < 1)
                    continue;
                string firstPart = s.Substring(0, idx);
                var splittedUnderscore = firstPart.Split('_');
                char firstAfterDot = s[idx+1];
                string newValue = s;
                if (!Char.IsUpper(firstAfterDot) || splittedUnderscore[0].ToUpperInvariant() != splittedUnderscore[0])
                {
                    if (!Char.IsUpper(firstAfterDot))
                        newValue = newValue.Replace($".{firstAfterDot}", $".{Char.ToUpper(firstAfterDot)}");
                    if (splittedUnderscore[0].ToUpperInvariant() != splittedUnderscore[0])
                        newValue = newValue.Replace(splittedUnderscore[0], splittedUnderscore[0].ToUpperInvariant());
                
                    result = result.Replace(s, newValue);
                    changeApplied = true;
                }
            }
            return result;
        }

        public static bool AreListsEqual(List<string> list1, List<string> list2)
        {
            return list1.Count == list2.Count
                && list1.OrderBy(s => s).SequenceEqual(list2.OrderBy(s => s));
        }

        public static List<string> ExtractStringsBetweenDollars(string input)
        {
            List<string> results = new List<string>();
            MatchCollection matches = Regex.Matches(input, @"\$(.*?)\$");

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    results.Add(match.Groups[1].Value);
                }
            }

            return results;
        }

        public static List<string> ExtractStringsBetweenSquare(string input)
        {
            List<string> results = new List<string>();
            MatchCollection matches = Regex.Matches(input, @"\[(.*?)\]");

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    results.Add(match.Groups[1].Value);
                }
            }

            return results;
        }

        public static List<Tuple<string, int>> ExtractStringsAndIndexBetweenSquare(string input)
        {
            List<Tuple<string, int>> results = new List<Tuple<string, int>>();
            MatchCollection matches = Regex.Matches(input, @"\[(.*?)\]");

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    results.Add(new Tuple<string, int>(match.Groups[1].Value, match.Index));
                }
            }

            return results;
        }
    }
}
