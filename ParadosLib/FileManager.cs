using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace ParadosLib
{
    internal static class FileManager
    {
        private const string SectionName = "SECTION FILE ";
        private const string Separator = " => ";
        private const string DbCsvFile = "HOI_IV_AllKeys_{0}_{1}.csv";

        public const string EmptyLine = "@@EMPTY";
        public const string CommentLine = "@@COMMENT";
        public const string LanguageFormat = "l_{0}:";
        public const string FileNameLanguageFormat = "l_{0}.yml";
        public const string FileNameSectionLanguageFormat = "{0}_l_{1}.yml";

        internal static string ExtractSectionFromFileName(string fileName, string language)
        {
            return fileName.Replace($"_{string.Format(FileNameLanguageFormat, language)}", string.Empty);
        }

        internal static Dictionary<string, Dictionary<string, ReadDto>> ExtractAllKeysFromFiles(DirectoryInfo originalLanguageDir, string originalLanguage, out int numeroChiavi, out Dictionary<string, List<ReadDto>> duplicateSectionList)
        {
            var originalSectionKeys = new Dictionary<string, Dictionary<string, ReadDto>>();
            duplicateSectionList = new Dictionary<string, List<ReadDto>>();

            FileInfo[] originalFiles = originalLanguageDir.GetFiles("*.yml");
            numeroChiavi = 0;

            foreach (FileInfo file in originalFiles)
            {
                var extracted = FileManager.ExtractKeysFromFile(file, originalLanguage, out int numeroChiaviVere, out List<ReadDto> duplicateList);
                numeroChiavi += numeroChiaviVere;
                originalSectionKeys.Add(ExtractSectionFromFileName(file.Name, originalLanguage), extracted);
                if (duplicateList.Count > 0)
                    duplicateSectionList.Add(file.Name, duplicateList);
            }

            return originalSectionKeys;
        }

        internal static Dictionary<string, ReadDto> ExtractKeysFromFile(FileInfo inputFile, string originalLanguage, out int numeroChiaviVere, out List<ReadDto> duplicateList)
        {
            var keyList = new Dictionary<string, ReadDto>();
            duplicateList = new List<ReadDto>();
            numeroChiaviVere = 0;
            using (StreamReader sr = new StreamReader(inputFile.FullName))
            {
                string line;
                int lineNumber = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    if (line == string.Format(LanguageFormat, originalLanguage))
                        continue;
                    if (string.IsNullOrWhiteSpace(line))
                        keyList.Add($"{EmptyLine}_{lineNumber}", ReadDto.Create($"{EmptyLine}_{lineNumber}", string.Empty));
                    else if (line.Contains("#"))
                        keyList.Add($"{CommentLine}_{lineNumber}", ReadDto.Create($"{CommentLine}_{lineNumber}", line.Trim()));
                    else
                    {
                        var indexEndkey = line.IndexOf(':');
                        string splittedValue = line.Substring(indexEndkey+1);
                        string splittedKey = line.Substring(0, indexEndkey);
                        int? rank = null;                        
                        if (splittedValue[0] != ' ')
                        {
                            rank = Convert.ToInt32(splittedValue[0].ToString());
                            splittedValue = splittedValue.Substring(1);
                        }
                        if (keyList.ContainsKey(splittedKey.Trim()))
                            duplicateList.Add(ReadDto.Create(splittedKey.Trim(), splittedValue.Trim(), rank));
                        else
                        {
                            string textClean = Helper.ApplyUpperCaseOnSquare(splittedValue.Trim().ReplaceQuotesEnsureStartEndsWithQuote(), out _);
                            keyList.Add(splittedKey.Trim(), ReadDto.Create(splittedKey.Trim(), textClean, rank));
                        }
                            
                        numeroChiaviVere++;
                    }
                }
            }
            return keyList;
        }

        internal static string GetStatus(KeyDto dto)
        {
            if (dto.MissingTraduction)
                return KeyStatusEnum.MISSING.ToString();
            if (dto.HasDifferences)
                return KeyStatusEnum.DIFFERS.ToString();
            return KeyStatusEnum.OK.ToString();
        }

        internal static KeyStatusEnum GetStatus(string input)
        {
            if (Enum.TryParse<KeyStatusEnum>(input, true, out KeyStatusEnum result))
                return result;
            return KeyStatusEnum.UNKNOWN;
        }

        internal static Dictionary<string, Dictionary<string, ReadDto>> ReadKeysFromDbFile(DirectoryInfo workingPath, string languageOrigin, string languageTraduced)
        {
            FileInfo input = new FileInfo(Path.Combine(workingPath.FullName, string.Format(DbCsvFile, languageOrigin, languageTraduced)));
            Dictionary<string, Dictionary<string, ReadDto>> allKeysFromDb = new Dictionary<string, Dictionary<string, ReadDto>>();

            //CSV in this form Section(files);Key;Rank;OriginalText(language);TranslatedText(language);Status
            // Section = 0; Key = 1, Rank = 2, OriginalText(language) = 3, TranslatedText(language) = 4, Status = 5
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                Quote = '"',
                Escape = '"',
                BadDataFound = context => { throw new Exception($"Error in csv {context.Field}"); }
            };

            using (StreamReader reader = new StreamReader(input.FullName, Encoding.UTF8))
            {
                using (CsvReader csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap(new DynamicCsvMap(languageOrigin, languageTraduced));
                    while (csv.Read())
                    {
                        var record = csv.GetRecord<ParadosCsvRecord>();
                        if (record == null)
                            continue;

                        //cerco la sezione corrispondente
                        string section = record.Section;
                        if (!allKeysFromDb.TryGetValue(section, out var sectionKeys))
                        {
                            sectionKeys = new Dictionary<string, ReadDto>();
                            allKeysFromDb.Add(section, sectionKeys);
                        }

                        //ora lavoro sulla sezione
                        KeyStatusEnum statusKey = GetStatus(record.Status);
                        if (statusKey == KeyStatusEnum.UNKNOWN)
                            throw new Exception($"Db file statusKey line number {csv.CurrentIndex} is UNKNOWN");

                        string key = record.Key;
                        if (string.IsNullOrEmpty(key))
                            throw new Exception($"Db file key line number {csv.CurrentIndex} is null or empty");
                        
                        int? rank = record.Rank == string.Empty ? null: Convert.ToInt32(record.Rank);

                        ReadDto readKey;
                        switch (statusKey)
                        {
                            case KeyStatusEnum.OK:
                                readKey = ReadDto.Create(key, record.TranslatedText.EnsureStartsEndsWithQuote(), rank);
                                break;
                            case KeyStatusEnum.MISSING:
                                readKey = ReadDto.Create(key, record.OriginalText.EnsureStartsEndsWithQuote(), rank);
                                break;
                            case KeyStatusEnum.DIFFERS: //warning ??
                                readKey = ReadDto.Create(key, record.TranslatedText.EnsureStartsEndsWithQuote(), rank);
                                break;
                            default:
                                throw new Exception($"Db file statusKey line number {csv.CurrentIndex} is UNKNOWN");
                        }
                        sectionKeys.Add(key, readKey);
                    }
                }                    
            }
            return allKeysFromDb;
        }

        internal static FileInfo WriteDbFiles(DirectoryInfo workingPath, string languageOrigin, string languageTraduced, Dictionary<string, Dictionary<string, KeyDto>> fileAllKeys)
        {
            FileInfo output = new FileInfo(Path.Combine(workingPath.FullName, string.Format(DbCsvFile, languageOrigin, languageTraduced)));
            //CSV in this form Section(files);Key;Rank;OriginalText(language);TranslatedText(language);Status
            using (StreamWriter writer = new StreamWriter(output.FullName, false, Encoding.UTF8))
            {
                writer.WriteLine($"Section(files);Key;Rank;OriginalText({languageOrigin});TranslatedText({languageTraduced});Status");
                foreach (var pair in fileAllKeys)
                {
                    foreach (var kvp in pair.Value)
                    {
                        if (kvp.Key.StartsWith(EmptyLine))
                            continue;
                        if (kvp.Key.StartsWith(CommentLine))
                            continue;
                        KeyDto dto = kvp.Value;
                        string rank = dto.Rank == null ? string.Empty : dto.Rank.ToString();
                        string status = GetStatus(dto);
                        writer.WriteLine($"{pair.Key};{kvp.Key};{rank};{dto.OriginalValue};{dto.TraducedValue};{status}");
                    }
                }
                writer.Close();
            }
            return output;
        }

        internal static int WriteFilesFromAllKeys(DirectoryInfo workingPath, string languageWrite, Dictionary<string, Dictionary<string, ReadDto>> fileAllKeys)
        {
            int fileWritten = 0;
            foreach (var pair in fileAllKeys)
            {
                WriteFileFromKeys(workingPath, languageWrite, pair.Key, pair.Value);
                fileWritten++;
            }
            return fileWritten;
        }

        internal static FileInfo WriteFileFromKeys(DirectoryInfo workingPath, string languageWrite, string sectionName, Dictionary<string, ReadDto> fileKeys)
        {
            DirectoryInfo workingLanguage = workingPath.CreateSubdirectory(languageWrite);
            FileInfo output = new FileInfo(Path.Combine(workingLanguage.FullName, string.Format(FileNameSectionLanguageFormat, sectionName, languageWrite)));

            using (StreamWriter writer = new StreamWriter(output.FullName, false, Encoding.UTF8))
            {
                writer.WriteLine(string.Format(LanguageFormat, languageWrite));                
                foreach (ReadDto kvp in fileKeys.Values)
                {
                    if (kvp.Key.StartsWith(EmptyLine))
                        writer.WriteLine(" "); //spazio
                    else if (kvp.Key.StartsWith(CommentLine))
                        writer.WriteLine($" {kvp.Text}");//spazio poi il commento
                    else
                    {
                        string rank = kvp.Rank == null ? string.Empty : kvp.Rank.ToString();
                        writer.WriteLine($" {kvp.Key}:{rank} {kvp.Text}");//spazio poi il commento
                    }
                }
                writer.Close();
            }
            return output;
        }
    }
}
