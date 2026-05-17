using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Microsoft.VisualBasic.FileIO;

public static class ModEntry
{
	private static string GAME_PATH => OS.GetExecutablePath().GetBaseDir();
	private static string MOD_PATH => Path.Combine(GAME_PATH, "mods/PratfallLocalizationMod/");
	private static string LOCALIZATION_CSV_PATH => Path.Combine(MOD_PATH, "Localization.csv");
	private const string MOD_DEBUG_PREFIX = "LOC_MOD: ";

	public static void ModInit()
	{
		ModPrint("Initializing Localization Mod...");
		CreateLocalizationsFromCSV();
	}

	private static void CreateLocalizationsFromCSV()
	{
		HashSet<string> loadedLocales = TranslationServer.GetLoadedLocales().ToHashSet();

		TextFieldParser parser = new(LOCALIZATION_CSV_PATH);
		parser.TextFieldType = FieldType.Delimited;
		parser.SetDelimiters(",");

		Dictionary<int, Translation> newTranslationsByIndex = new();
		bool hasReadHeader = false;

		while (!parser.EndOfData) 
		{
			string[] fields = parser.ReadFields();
			if (!hasReadHeader)
			{
				for (int i = 1; i < fields.Length; i++)
				{
					string locale = fields[i];
					if (loadedLocales.Contains(locale))
					{
						ModPrint($"Ignoring locale '{locale}'. It already exists.");
						continue;
					}

					ModPrint($"Found new locale '{locale}'");

					Translation translation = new() { Locale = locale };
					newTranslationsByIndex.Add(i, translation);
				}

				hasReadHeader = true;

				if (newTranslationsByIndex.Count == 0)
				{
					ModPrint("Couldn't find any new locale.");
					return;
				}
			}
			else
			{
				foreach (string field in fields)
				{
					string key = fields[0];
					foreach (var translation in newTranslationsByIndex)
					{
						string localizedField = fields[translation.Key];
						translation.Value.AddMessage(key, localizedField);
					}
				}
			}
		}

		if (newTranslationsByIndex.Count == 0)
		{
			ModPrint("Couldn't read .csv file");
			return;
		}

		foreach (var translation in newTranslationsByIndex)
			TranslationServer.AddTranslation(translation.Value);
			
		TranslationServer.SetLocale(TranslationServer.GetLocale());
		Translation testTranslation = newTranslationsByIndex.First().Value;
		ModPrint($"Changing locale from to '{testTranslation.Locale}'");
		TranslationServer.SetLocale(testTranslation.Locale);

		ModPrint("Loaded and applied localizaiton.");
	}

	public static void ModDestroy()
	{
		ModPrint("Localization Mod destroyed!");
	}

	public static void ModPrint(string message)
	{
		GD.Print($"{MOD_DEBUG_PREFIX}{message}");
	}
}
