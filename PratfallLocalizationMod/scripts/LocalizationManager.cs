using System.Collections.Generic;
using System.IO;
using Godot;
using Microsoft.VisualBasic.FileIO;

public static class LocalizationManager 
{
	private static string LOCALIZATION_FOLDER => Path.Combine(ModUtils.MOD_PATH, "Localization");
	private static Dictionary<string, Translation> _customTranslationByLocale = new();
	public static bool HasJustSetLanguage;
	private static string _initialLocale;
	public static string SystemLocale = "en";

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
		_initialLocale = TranslationServer.GetLocale();
		Config.Load();
		AddTranslationsFromCSVFolder();
		TrySetInitialLanguage();
	}

	public static void Destroy()
	{
		ResetToNonCustomTranslation();
		RemoveAddedTranslations();
	}

	private static void AddTranslationsFromCSVFolder()
	{
		string[] csvFiles = Directory.GetFiles(LOCALIZATION_FOLDER, "*.csv");
		foreach (var csv in csvFiles)
			AddTranslationsFromCSV(csv);
	}

	private static void AddTranslationsFromCSV(string csvPath)
	{
		ModUtils.Print($"Reading {Path.GetFileName(csvPath)}");
		
		using TextFieldParser parser = new(csvPath);
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
					if (TranslationServer.HasTranslationForLocale(locale, exact: true))
					{
						ModUtils.Print($"Locale '{locale}' already has a translation. Ignoring...");
						continue;
					}

					ModUtils.Print($"Found new locale '{locale}'");

					Translation translation = new() { Locale = locale };
					newTranslationsByIndex.Add(i, translation);
				}

				hasReadHeader = true;

				if (newTranslationsByIndex.Count == 0)
					return;
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
			return;

		foreach (var translation in newTranslationsByIndex)
		{
			TranslationServer.AddTranslation(translation.Value);
			_customTranslationByLocale.Add(translation.Value.Locale, translation.Value);
		}
	}

	public static void TrySetInitialLanguage()
	{
		if (Config.Data.AutoLoadCustomLocale)
			SetLanguage(Config.Data.SelectedCustomLocale);
	}

	private static void SetLanguage(string locale)
	{
		if (!TranslationServer.HasTranslationForLocale(locale, exact: true))
			return;

		ModUtils.Print($"Setting language to '{locale}'");
		HasJustSetLanguage = true;
		TranslationServer.SetLocale(locale);
		HandleLanguageChange();
	}

	private static bool IsCustomLocale(string locale)
	{
		return _customTranslationByLocale.ContainsKey(locale);
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

	public static void RemoveAddedTranslations()
	{

		foreach (var customTranslation in _customTranslationByLocale)
			TranslationServer.RemoveTranslation(customTranslation.Value);

		_customTranslationByLocale.Clear();
	}

	private static void ResetToNonCustomTranslation()
	{
		string currentLocale = TranslationServer.GetLocale();
		if (!IsCustomLocale(currentLocale))
			return;

		string resetLocale = IsCustomLocale(_initialLocale) ? SystemLocale : _initialLocale;
		ModUtils.Print($"Resetting language to {resetLocale}");
		HasJustSetLanguage = true;
		TranslationServer.SetLocale(resetLocale);
	}
}
