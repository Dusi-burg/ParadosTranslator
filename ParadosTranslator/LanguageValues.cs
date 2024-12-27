using System.Configuration;

namespace ParadosTranslator
{
    public static class LanguageValues
    {
        public static List<string> Values { get; set; }

        static LanguageValues() 
        {
            string allLanguage = ConfigurationManager.AppSettings["Languages"];
            if (string.IsNullOrEmpty(allLanguage))
                throw new Exception("Languages not found in app.config.");

            Values = allLanguage.Split(',').ToList();
        }
    }
}
