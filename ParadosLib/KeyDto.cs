
namespace ParadosLib
{
    public class KeyDto
    {
        public static KeyDto Create(string key, string originalValue, string traducedValue, bool hasDifferences, int? rank = null)
        {
            return new KeyDto(key, originalValue, traducedValue, rank, hasDifferences, false);
        }

        public static KeyDto Create(ReadDto readDto)
        {
            return new KeyDto(readDto.Key, readDto.Text, string.Empty, readDto.Rank, false, true);
        }

        public KeyDto(string key, string originalValue, string traducedlValue, int? rank = null, bool hasDifferences = false, bool missingTraduction = false)
        {
            Key = key;
            OriginalValue = originalValue;
            TraducedValue = traducedlValue;
            Rank = rank;
            HasDifferences = hasDifferences;
            MissingTraduction = missingTraduction;
        }

        public string Key { get; set; }

        public bool HasDifferences { get; set; }

        public bool MissingTraduction { get; set; }

        public string OriginalValue { get; set; }
        public int? Rank { get; set; }
        public string TraducedValue { get; set; }
    }
}
