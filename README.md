# Parados Translator
This is a simple tool that help anyone want to translate Paradox games into different unsupported languages (es Italian).
It compare a original folder (es english) with a tanslated ones, also of different version, to create a db file in csv format to check the translation with an Excel sheet.
The created csv is a collectionof keys with a status of OK (found and correct), MISSING (missing in translation) and DIFFERS (key is found but metadata is wrong).
You can edit all the DIFFERS and MISSING keys and once the work is done you can re-create the language games files using dpecs from original and translated keys from csv file.

# Parados UI
<img src="Parados%20UI.png">

First select orginal folder and language, than a translated folder and translated language, then select a working folder.
First step you can verify aligniment between original and translated files by clicking on "Verify ORI-TRA" button, then you can create db (csv) file by clicking "Create Db" button.
Adjust tranlastion missing or differs and then when all is done, you can create translated files "Create Files From Db" button,
all selected language files are created in working folder.

# Csv Db File
For each test key found in every files of original language a row is created in csv file.
Comment and emtpy lines are skipped.

## Csv Columns

|                |MEANS
|----------------|-----
|Section(files)|`Mane of the files containing key (without .yml extension)`
|Key|`Key value`
|Rank|`an optional integer found in original files`
|OriginalText(english)|`The original text`
|TranslatedText(english)|`The translated text`
|Status|`The row status (see below)`

## Csv Status

The single row in csv have a status:

|                |MEANS
|----------------|-----
|OK|`Key is found and semantic [variable in square] are all ok`
|MISSING|`Key is not found`
|DIFFERS|`Key is found but semantic [variable in square] have one or more differencies`

# Build and run
Install .Net 8 SDK see https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.404-windows-x64-installer then open a prompt in the root folder and simple run "dotnet build" command. The output will be in ParadosTranslator\bin\Debug\net8.0-windows foler double click on ParadosTranslator.exe and run it.
