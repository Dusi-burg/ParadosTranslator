using CsvHelper.Configuration;

namespace ParadosLib
{
    public class ParadosCsvRecord
    {
        //Section;Key;Rank;OriginalText(language); TraducedText(language); Status
        public string Section { get; set; }
        public string Key { get; set; }
        public string Rank { get; set; }
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
        public string Status { get; set; }
    }

    public class DynamicCsvMap : ClassMap<ParadosCsvRecord>
    {
        public DynamicCsvMap(string originalLanguage, string traducedLanguage)
        {
            Map(m => m.Section).Name("Section(files)");
            Map(m => m.Key).Name("Key");
            Map(m => m.OriginalText).Name($"OriginalText({originalLanguage})");
            Map(m => m.TranslatedText).Name($"TranslatedText({traducedLanguage})");
            Map(m => m.Status).Name("Status");
        }
    }
}
