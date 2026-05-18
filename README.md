# Pratfall Localization Mod
This mod reads custom localizations from a `.csv` file and injects them into Pratfall.

## Installation
- [Download](https://github.com/HenriqueCamillo/PratfallLocalizationMod/releases/tag/1.0.0) the mod and extract it to the `mods` folder located in Pratfall's installation folder.
- Add a file named `Localization.csv` containing the custom localizations to the `PratfallLocalizationMod` folder 
- Enable the mod from the in-game mods menu

Your folder structure should be like this:
```
Pratfall
└ mods
ㅤ└ PratfallLocalizationMod/
ㅤㅤ├ Localization.csv
ㅤㅤ├ manifest.json
ㅤㅤ├ PratfallLocalizationMod.dll
ㅤㅤ└ PratfallLocalizationMod.pck
```
## Adding new languages
To add new languages, you need a `Localization.csv` file with the following format:
- The first line should be a header containing the locales
- The first column should contain the keys for the localization strings (which are the English strings in this game)
- The following columns should contain the translations for each language

Note: Additional columns with the locales already existing in the game will be ignored (so you can keep the original translations in the file)

You can use [this template](https://docs.google.com/spreadsheets/d/159jjgwiimEFuYmiFUlIKS75MIr6kHpIYdULLQ4FOtv0/edit?usp=sharing), it already contains the localization keys and the original translations, so you just need to add new columns for the new languages. Just keep in mind it may get outdated with game updates.

## Switching languages
The mod already sets the game to the first custom language on startup, but it also overrides the language selector behaviour, showing a custom language every three language changes, cycling through all of them.

## Extracting the localizations from the game
If you want to extract the most recent localizations from the game yourself, you can use [Godot PCK Explorer](https://github.com/DmitriySalnikov/GodotPCKExplorer) to extract the `.po` files from the `Pratfall.pck` (located in the game's installation folder). They are located in `data/localization`. Once you have extracted a `.po` file, you can use a converter such as [csv2po](https://docs.translatehouse.org/projects/translate-toolkit/en/latest/commands/csv2po.html) to convert it to `.csv`, and then you are ready to add new languages.
