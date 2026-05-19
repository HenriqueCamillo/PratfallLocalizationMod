using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Microsoft.VisualBasic.FileIO;

public static class LocalizationManager 
{
	private static string LOCALIZATION_CSV_PATH => Path.Combine(ModUtils.MOD_PATH, "Localization.csv");
	private static List<string> _customLocales = new();
	private static int _currentLocaleIndex = -1;
	private static bool _isInitialized;
	public static bool IsCustomLanguageActive;

	public static void Init()
	{
		if (!_isInitialized)
		{
			CreateLocalizationsFromCSV();
			_isInitialized = true;
		}
  
		SetLanguageToCustomLocale(0);
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
						ModUtils.Print($"Ignoring locale '{locale}'. It already exists.");
						continue;
					}

					ModUtils.Print($"Found new locale '{locale}'");

					Translation translation = new() { Locale = locale };
					newTranslationsByIndex.Add(i, translation);
				}

				hasReadHeader = true;

				if (newTranslationsByIndex.Count == 0)
				{
					ModUtils.Print("Couldn't find any new locale.");
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
			ModUtils.Print("Couldn't read .csv file");
			return;
		}

		foreach (var translation in newTranslationsByIndex)
		{
			TranslationServer.AddTranslation(translation.Value);
			_customLocales.Add(translation.Value.Locale);
		}
	}

	public static void SetLanguageToNextLocale() => SetLanguageToCustomLocale(_currentLocaleIndex + 1);

	public static void SetLanguageToCustomLocale(int localeIndex)
	{
		if (_customLocales.Count == 0)
			return;

		localeIndex %= _customLocales.Count;
		localeIndex = Mathf.Clamp(localeIndex, 0, _customLocales.Count - 1);
		if (IsCustomLanguageActive && localeIndex == _currentLocaleIndex)
			return;

		_currentLocaleIndex = localeIndex;
		string locale = _customLocales[_currentLocaleIndex];
		TranslationServer.SetLocale(locale);

		IsCustomLanguageActive = true;
		ModUtils.Print($"Set language to '{locale}'");
	}
}
