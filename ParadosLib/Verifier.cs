﻿
namespace ParadosLib
{
    public class Verifier
    {
        public List<string> MatchOriginalVsTranslated(
          string originFolderPath,
          string originalLanguage,
          string traducedFolderPath,
          string traducedLanguage,
          string workingFolderPath,
          string workingLanguage,
          bool writeDbCsv)
        {
            List<string> operations = new List<string>();
            //la folder di origin prevede in input un linguaggio e cerca la folder con quel nome, mentre crea un linguaggio in uscita usando la working folder

            DirectoryInfo originDirectory = new DirectoryInfo(originFolderPath);
            DirectoryInfo traducedDirectory = new DirectoryInfo(traducedFolderPath);
            DirectoryInfo workingDirectory = new DirectoryInfo(workingFolderPath);
            DirectoryInfo originalLanguageDir = originDirectory.GetDirectories(originalLanguage).SingleOrDefault();
            DirectoryInfo traducedLanguageDir = traducedDirectory.GetDirectories(traducedLanguage).SingleOrDefault();

            operations.Add($"Reading language {originalLanguage} from folder {originalLanguageDir}");
            //STEP 1
            //creo un dizionario per ogni Sezione che è un file, poi un dizionario per ogni chiave-valore di traduzione
            Dictionary<string, Dictionary<string, ReadDto>> originalSectionKeys = FileManager.ExtractAllKeysFromFiles(originalLanguageDir, originalLanguage, out int numeroChiaviOrig, out Dictionary<string, List<ReadDto>> duplicateSectionListOriginal);
            if (CheckDuplicateKey(originalLanguage, operations, duplicateSectionListOriginal))
                return operations;

            operations.Add($"Found {originalSectionKeys.Count} files from original");
            operations.Add($"Found {numeroChiaviOrig} keys from original (excluding empty lines and comment)");
            //operations.Add($"Found {originalSectionKeys.Sum(kv => kv.Value.Count(c => !(c.Key.StartsWith(FileManager.EmptyLine) || c.Key.StartsWith(FileManager.CommentLine))))} keys from original");

            operations.Add($"Reading language {traducedLanguage} from folder {traducedFolderPath}");
            //STEP 1
            //creo un dizionario per ogni Sezione che è un file, poi un dizionario per ogni chiave-valore di traduzione
            Dictionary<string, Dictionary<string, ReadDto>> traducedSectionKeys = FileManager.ExtractAllKeysFromFiles(traducedLanguageDir, traducedLanguage, out int numeroChiaviTraduced, out Dictionary<string, List<ReadDto>> duplicateSectionListTraduced);
            if (CheckDuplicateKey(originalLanguage, operations, duplicateSectionListTraduced))
                return operations;

            operations.Add($"Found {traducedSectionKeys.Count} files from translated");
            operations.Add($"Found {numeroChiaviTraduced} keys from translated (excluding empty lines and comment)");
            operations.Add($"Found {traducedSectionKeys.Sum(kv => kv.Value.Count(c=>!(c.Key.StartsWith(FileManager.EmptyLine)|| c.Key.StartsWith(FileManager.CommentLine))))} keys from translated");

            Dictionary<string, Dictionary<string, KeyDto>> missingSections = originalSectionKeys.ExceptBy(traducedSectionKeys.Keys, s => s.Key).ToDictionary(k => k.Key, v => v.Value.ToDictionary(l => l.Key, p => KeyDto.Create(p.Value)));

            operations.Add($"Found {missingSections.Count} mssing files from translated");
            operations.Add($"Found {missingSections.Sum(kv => kv.Value.Count(c => !(c.Key.StartsWith(FileManager.EmptyLine) || c.Key.StartsWith(FileManager.CommentLine))))} mssing keys from translated");

            Dictionary<string, Dictionary<string, KeyDto>> missingSectionsClean = new Dictionary<string, Dictionary<string, KeyDto>>();
            foreach (var kvp in missingSections)
            {
                missingSectionsClean.Add(kvp.Key, kvp.Value.Where(c => !(c.Key.StartsWith(FileManager.EmptyLine) || c.Key.StartsWith(FileManager.CommentLine))).ToDictionary(k => k.Key, v => v.Value));
            }

            //logica match
            Dictionary<string, Dictionary<string, ReadDto>> commonSections = originalSectionKeys.IntersectBy(traducedSectionKeys.Keys, s => s.Key).ToDictionary(k => k.Key, v => v.Value);
            Dictionary<string, Dictionary<string, KeyDto>> compatibleSectionKeys = new Dictionary<string, Dictionary<string, KeyDto>>();
            Dictionary<string, Dictionary<string, KeyDto>> nonCompatibleSectionKeys = new Dictionary<string, Dictionary<string, KeyDto>>();
            Dictionary<string, Dictionary<string, KeyDto>> missingSectionKeys = new Dictionary<string, Dictionary<string, KeyDto>>();
            Dictionary<string, Dictionary<string, KeyDto>> commonSectionsResultKeys = new Dictionary<string, Dictionary<string, KeyDto>>();

            int sezioniViste = 0;
            int chiaviViste = 0;
            foreach (var sectionInfo in commonSections)
            {
                sezioniViste++;
                Dictionary<string, KeyDto> compatibleKey = new Dictionary<string, KeyDto>();
                Dictionary<string, KeyDto> nonCompatibleKey = new Dictionary<string, KeyDto>();
                Dictionary<string, KeyDto> missingKey = new Dictionary<string, KeyDto>();
                var traducedDictionary = traducedSectionKeys[sectionInfo.Key];
                foreach (var kvp in sectionInfo.Value)
                {
                    if (kvp.Key.StartsWith(FileManager.EmptyLine) || kvp.Key.StartsWith(FileManager.CommentLine))
                        continue;
                    chiaviViste++;
                    if (traducedDictionary.TryGetValue(kvp.Key, out var value))
                    {
                        if (Helper.TraductionIsCompatible(kvp.Value.Text, value.Text))
                            compatibleKey.Add(kvp.Key, KeyDto.Create(kvp.Key, kvp.Value.Text, value.Text, false, kvp.Value.Rank));
                        else
                        {
                            //provo ad aggiustare
                            if (Helper.ApplySquareAdjustament(kvp.Value.Text, value.Text, out string adjustedTraduced))
                            {
                                if (Helper.TraductionIsCompatible(kvp.Value.Text, adjustedTraduced))
                                    compatibleKey.Add(kvp.Key, KeyDto.Create(kvp.Key, kvp.Value.Text, adjustedTraduced, false, kvp.Value.Rank));
                                else
                                    nonCompatibleKey.Add(kvp.Key, KeyDto.Create(kvp.Key, kvp.Value.Text, adjustedTraduced, true, kvp.Value.Rank));
                            }
                            else
                                nonCompatibleKey.Add(kvp.Key, KeyDto.Create(kvp.Key, kvp.Value.Text, value.Text, true, kvp.Value.Rank));
                        }

                    }
                    else
                        missingKey.Add(kvp.Key, KeyDto.Create(kvp.Value));

                }
                commonSectionsResultKeys.Add(sectionInfo.Key, compatibleKey.Union(nonCompatibleKey).Union(missingKey).ToDictionary(k => k.Key, v => v.Value));
                if (compatibleKey.Count > 0)
                    compatibleSectionKeys.Add(sectionInfo.Key, compatibleKey);
                if (nonCompatibleKey.Count > 0)
                    nonCompatibleSectionKeys.Add(sectionInfo.Key, nonCompatibleKey);
                if (missingKey.Count > 0)
                    missingSectionKeys.Add(sectionInfo.Key, missingKey);
            }

            operations.Add($"Found {sezioniViste} sections with {chiaviViste} keys resulting from match original-translated");

            operations.Add($"Found {commonSections.Count} common files from translated");
            operations.Add($"Found {compatibleSectionKeys.Sum(kv => kv.Value.Count)} compatible keys from translated");
            operations.Add($"Found {nonCompatibleSectionKeys.Sum(kv => kv.Value.Count)} non-compatible keys from translated");
            operations.Add($"Found {missingSectionKeys.Sum(kv => kv.Value.Count)} missing keys from translated");

            if (!writeDbCsv)
                return operations;

            //scrivo il db csv
            Dictionary<string, Dictionary<string, KeyDto>> allSectionKeys = missingSectionsClean.Union(commonSectionsResultKeys)
                .OrderBy(s=>s.Key).ToDictionary(k => k.Key, v => v.Value);

            operations.Add($"Beging writing {allSectionKeys.Count} sections (files) with total {allSectionKeys.Sum(kv => kv.Value.Count)} keys in db (csv)");

            var result = FileManager.WriteDbFiles(workingDirectory, originalLanguage, workingLanguage, allSectionKeys);
            
            operations.Add($"Db (csv) {result.FullName} written succesfully.");
            return operations;
        }

        public List<string> CreateFromDb(
          string originFolderPath,
          string originalLanguage,         
          string workingFolderPath,
          string traducedLanguage)
        {
            List<string> operations = new List<string>();

            DirectoryInfo originDirectory = new DirectoryInfo(originFolderPath);
            DirectoryInfo workingDirectory = new DirectoryInfo(workingFolderPath);
            DirectoryInfo originalLanguageDir = originDirectory.GetDirectories(originalLanguage).SingleOrDefault();

            operations.Add($"Reading language {originalLanguage} from folder {originalLanguageDir}");

            //STEP 1
            //creo un dizionario per ogni Sezione che è un file, poi un dizionario per ogni chiave-valore di traduzione
            Dictionary<string, Dictionary<string, ReadDto>> originalSectionKeys = FileManager.ExtractAllKeysFromFiles(originalLanguageDir, originalLanguage, out int numeroChiaviOrig, out Dictionary<string, List<ReadDto>> duplicateSectionListOriginal);
            if (CheckDuplicateKey(originalLanguage, operations, duplicateSectionListOriginal))
                return operations;

            operations.Add($"Found {originalSectionKeys.Count} files from original");
            operations.Add($"Found {numeroChiaviOrig} keys from original (excluding empty lines and comment)");
            //operations.Add($"Found {originalSectionKeys.Sum(kv => kv.Value.Count(c => !(c.Key.StartsWith(FileManager.EmptyLine) || c.Key.StartsWith(FileManager.CommentLine))))} keys from original");

            operations.Add($"Reading db (csv) files {originalLanguage}-{traducedLanguage} in {workingFolderPath})");

            Dictionary<string, Dictionary<string, ReadDto>> traducedSectionKeys = FileManager.ReadKeysFromDbFile(workingDirectory, originalLanguage, traducedLanguage);

            //matcho sezioni e chiavi
            if (!originalSectionKeys.Keys.Order().SequenceEqual(traducedSectionKeys.Keys.Order()))
            {
                operations.Add($"Section mismatch between original files {originalLanguage} from folder {originalLanguageDir} and db (csv) file {traducedLanguage} in {workingDirectory})");
                var missingSections = originalSectionKeys.Keys.Except(traducedSectionKeys.Keys).ToList();
                return operations;
            }
            foreach (var pairSectionKey in originalSectionKeys)
            {
                var traducedSection = traducedSectionKeys[pairSectionKey.Key];
                foreach (var kvp in pairSectionKey.Value)
                {
                    if (kvp.Key.StartsWith(FileManager.EmptyLine) || kvp.Key.StartsWith(FileManager.CommentLine))
                        continue;
                    //altrimenti sostituisco il valore testo
                    if (traducedSection.ContainsKey(kvp.Key))
                        kvp.Value.Text = traducedSection[kvp.Key].Text;
                    else
                        operations.Add($"Missing translated key {kvp.Key} in section {pairSectionKey.Key}");
                }
            }

            operations.Add($"Writing yml files {traducedLanguage} in {workingDirectory})");

            int fileWritten = FileManager.WriteFilesFromAllKeys(workingDirectory, traducedLanguage, originalSectionKeys);
            operations.Add($"Writtend {fileWritten} yml files of {traducedLanguage} in {workingDirectory})");

            return operations;
        }

        private bool CheckDuplicateKey(string originalLanguage, List<string> operations, Dictionary<string, List<ReadDto>> duplicateSectionListOriginal)
        {
            if (duplicateSectionListOriginal.Count > 0)
            {
                operations.Add($"ERROR found DUPLICATED ORIGINAL KEY IN {originalLanguage}");
                foreach (var section in duplicateSectionListOriginal)
                {
                    foreach (var valore in section.Value)
                    {
                        operations.Add($"File {section.Key} valore {valore.Key}");
                    }
                }
                return true;
            }
            return false;
        }

        #region old

        //public List<string> MatchOriginalVsDb(
        //  string originFolderPath,
        //  string originalLanguage,
        //  string outputLanguage,
        //  string workingFolderPath)
        //{
        //    List<string> operations = new List<string>();
        //    //la folder di origin prevede in input un linguaggio e cerca la folder con quel nome, mentre crea un linguaggio in uscita usando la working folder

        //    DirectoryInfo originDirectory = new DirectoryInfo(originFolderPath);
        //    DirectoryInfo workingDirectory = new DirectoryInfo(workingFolderPath);
        //    DirectoryInfo originalLanguageDir = originDirectory.GetDirectories(originalLanguage).SingleOrDefault();
        //    workingDirectory.CreateSubdirectory(outputLanguage);

        //    operations.Add($"Lettura linguaggio {originalLanguage} da folder {originalLanguageDir}");
        //    //Path.Combine(originDirectory
        //    //STEP 1
        //    //creo un dizionario per ogni Sezione che è un file, poi un dizionario per ogni chiave-valore di traduzione
        //    Dictionary<string, Dictionary<string, ReadDto>> originalSectionKeys = FileManager.ExtractAllKeysFromFiles(originalLanguageDir, originalLanguage, out int numeroChiavi, out Dictionary<string, List<ReadDto>> duplicateSectionListOriginal);
        //    if (CheckDuplicateKey(originalLanguage, operations, duplicateSectionListOriginal))
        //        return operations;

        //    Dictionary<string, ReadDto> originalCrushedKeys = originalSectionKeys.SelectMany(kv => kv.Value)
        //        .Where(sb => !(sb.Key.StartsWith(FileManager.EmptyLine) || sb.Key.StartsWith(FileManager.CommentLine)))
        //        .ToDictionary(subKv => subKv.Key, subKv => subKv.Value);

        //    //STEP 2 leggo dal db delle chiavi tradotte
        //    Dictionary<string, Dictionary<string, KeyDto>> dbSectionKeys = FileManager.ReadFromOldDbFile(workingDirectory, originalLanguage);
        //    Dictionary<string, Dictionary<string, ReadDto>> missingSectionKeys = new Dictionary<string, Dictionary<string, ReadDto>>();
        //    Dictionary<string, Dictionary<string, KeyDto>> differsSectionKeys = new Dictionary<string, Dictionary<string, KeyDto>>();
        //    int numeroChiaviMissing = 0;
        //    int numeroChiaviDiffers = 0;

        //    int numeroChiaviCrushedDiffers = 0;

        //    Dictionary<string, KeyDto> dbCrushedKeys = dbSectionKeys.SelectMany(kv => kv.Value)
        //        .Where(sb => !(sb.Key.StartsWith(FileManager.EmptyLine) || sb.Key.StartsWith(FileManager.CommentLine)))
        //        .ToDictionary(subKv => subKv.Key, subKv => subKv.Value);

        //    //provo a valutare l'origin delle traduzioni che hanno la stessa chiave, per confermare che anche se su files diversi sono la stessa cosa
        //    var chiaviCrushedDiffers = originalCrushedKeys.Keys.Except(dbCrushedKeys.Keys);
        //    numeroChiaviCrushedDiffers = chiaviCrushedDiffers.Count();

        //    int numeroChiaviCrushedSameKeyDifferentText = 0;
        //    int numeroChiaviCrushedSameKeyContainedText = 0;

        //    Dictionary<string, ReadDto> crushedIntersect = originalCrushedKeys.IntersectBy(dbCrushedKeys.Keys, s => s.Key).ToDictionary(k => k.Key, v => v.Value);
        //    foreach (var kvp in crushedIntersect)
        //    {
        //        var translated = dbCrushedKeys[kvp.Key];
        //        if (translated != null)
        //        {
        //            if (!translated.OriginalValue.Contains(kvp.Value.Text))
        //                numeroChiaviCrushedSameKeyDifferentText++;
        //            else
        //                numeroChiaviCrushedSameKeyContainedText++;
        //        }
        //    }

        //    operations.Add($"Found {originalSectionKeys.Count} files");
        //    operations.Add($"Found {numeroChiavi} chiavi (excludendo linee vuote e commenti)");

        //    operations.Add($"Found {numeroChiaviCrushedDiffers} chiavi mancanti schiacciate senza contare la sections.");
        //    operations.Add($"Found {numeroChiaviCrushedSameKeyDifferentText} chiavi uguali schiacciate senza contare la sections ma con testo originale diverso.");
        //    operations.Add($"Found {numeroChiaviCrushedSameKeyContainedText} chiavi uguali schiacciate senza contare la sections e con testo almeno contenuto o uguale.");

        //    foreach (var pairSection in originalSectionKeys)
        //    {
        //        if (dbSectionKeys.ContainsKey(pairSection.Key))
        //        {
        //            Dictionary<string, KeyDto> currentDb = dbSectionKeys[pairSection.Key];
        //            Dictionary<string, ReadDto> currentMissingKeys = new Dictionary<string, ReadDto>();
        //            Dictionary<string, KeyDto> currentdiffersKeys = new Dictionary<string, KeyDto>();

        //            //verifico le chiavi
        //            foreach (var pairValue in pairSection.Value)
        //            {
        //                if (currentDb.TryGetValue(pairValue.Key, out KeyDto keyDto)) //se trovato verifico siano compatibili
        //                {
        //                    keyDto.HasDifferences = !Helper.TraductionIsCompatible(pairValue.Value.Text, keyDto.TraducedValue);
        //                    keyDto.OriginalValue = pairValue.Value.Text;
        //                    keyDto.Rank = pairValue.Value.Rank;
        //                    if (keyDto.HasDifferences)
        //                        currentdiffersKeys.Add(pairValue.Key, keyDto);
        //                }
        //                else
        //                    currentMissingKeys.Add(pairValue.Key, pairValue.Value);
        //            }
        //            if (currentMissingKeys.Count > 0)
        //            {
        //                missingSectionKeys.Add(pairSection.Key, currentMissingKeys);
        //                numeroChiaviMissing += currentMissingKeys.Count;
        //            }
        //            if (currentdiffersKeys.Count > 0)
        //            {
        //                differsSectionKeys.Add(pairSection.Key, currentdiffersKeys);
        //                numeroChiaviDiffers += currentdiffersKeys.Count;
        //            }
        //        }
        //        else 
        //        {
        //            missingSectionKeys.Add(pairSection.Key, pairSection.Value);
        //            numeroChiaviMissing += pairSection.Value.Count;
        //        }
        //    }
        //    operations.Add($"Found {missingSectionKeys.Count} missing sections con {numeroChiaviMissing} chiavi mancanti.");
        //    operations.Add($"Found {differsSectionKeys.Count} differenti sections con {numeroChiaviDiffers} chiavi differenti.");

        //    return operations;
        //}

        #endregion
    }

}