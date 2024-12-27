
namespace ParadosLib
{
    public class ReadDto
    {
        public static ReadDto Create(string key, string text, int? rank = null)
        {
            return new ReadDto(key, text, rank);
        }

        public ReadDto(string key, string text, int? rank)
        {
            Text = text;
            Key = key;
            Rank = rank;
        }

        public string Text { get; set; }

        public string Key { get; set; }

        public int? Rank { get; set; }
    }
}
