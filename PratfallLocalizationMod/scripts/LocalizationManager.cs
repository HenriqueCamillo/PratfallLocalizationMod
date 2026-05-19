using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Microsoft.VisualBasic.FileIO;

public static class LocalizationManager 
{
	private static string LOCALIZATION_CSV_PATH => Path.Combine(ModUtils.MOD_PATH, "Localization.csv");
	private static HashSet<string> _customLocales = new();
	private static bool _isInitialized;
	public static bool HasJustSetLanguage;

	public static bool _isCustomLanguageActive;
	public static bool IsCustomLanguageActive
	{
		get => _isCustomLanguageActive;
		set
		{
			_isCustomLanguageActive = value;
			if (Config.Data.AutoLoadCustomLocale == _isCustomLanguageActive)
				return;

			Config.Data.AutoLoadCustomLocale = _isCustomLanguageActive;
			Config.Save();
		}
	}

	public static void Init()
	{
		if (!_isInitialized)
		{
			Config.Load();
			CreateLocalizationsFromCSV();
			_isInitialized = true;
		}
  
		TrySetInitialLanguage();
	}

	public static void TrySetInitialLanguage()
	{
		if (Config.Data.AutoLoadCustomLocale)
			SetLanguage(Config.Data.SelectedCustomLocale);
	}

	private static void CreateLocalizationsFromCSV()
	{
		HashSet<string> loadedLocales = TranslationServer.GetLoadedLocales().ToHashSet();

		using TextFieldParser parser = new(LOCALIZATION_CSV_PATH);
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
			ModUtils.Print("Invalid .csv file");
			return;
		}

		foreach (var translation in newTranslationsByIndex)
		{
			TranslationServer.AddTranslation(translation.Value);
			_customLocales.Add(translation.Value.Locale);
		}
	}

	private static void SetLanguage(string locale)
	{
		if (!TranslationServer.HasTranslationForLocale(locale, exact: true))
		{
			ModUtils.Print($"Trying to set language to unavailable locale: '{locale}'");
			return;
		}

		ModUtils.Print($"Setting language to '{locale}'");
		HasJustSetLanguage = true;
		TranslationServer.SetLocale(locale);
		HandleLanguageChange();
	}

	private static bool IsCustomLocale(string locale)
	{
		return _customLocales.Contains(locale);
	}

	public static void HandleLanguageChange()
	{
		string locale = TranslationServer.GetLocale();
		IsCustomLanguageActive = IsCustomLocale(locale);

		if (IsCustomLanguageActive)
		{
			Config.Data.SelectedCustomLocale = locale;
			Config.Save();
		}
	}
}
