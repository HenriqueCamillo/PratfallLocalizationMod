# Pratfall Localization Mod
This mod reads custom localizations from `.csv` files and injects them into Pratfall.

Note: This mod by itself doesn't add any custom language, see the `Adding New Languages` section to understand how it works.

## Installation
### Steam Workshop
- Subscribing to this mod on [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3729364926)
- Add new languages (see next section)
- Enable the mod from the in-game mods menu

### Manual Installation
- [Download](https://github.com/HenriqueCamillo/PratfallLocalizationMod/releases/) the mod and extract it to the `mods` folder located in Pratfall's installation folder.
- Add new languages (see next section)
- Enable the mod from the in-game mods menu

## Adding New Languages
### Language Pack Mods
You can download language mod packs that inject content to this mod. You can also make your own Language Pack Mod using the [Content Injector template](https://github.com/HenriqueCamillo/PratfallLocalizationModContentInjector).

Known language packs:
- [Esperanto](https://steamcommunity.com/sharedfiles/filedetails/?id=3729365046)


### Manually Adding Your Languages
You can also add your own languages by adding `.csv` files to the `Localization` folder. If you manually installed the mod, it will be located in `Pratfall/mods/PratfallLocalizationMod`, and if you downloaded it from Steam Workshop, on `{SteamInstallationFolder}\Steam\steamapps\workshop\content\4244510\3729364926`.

The `.csv` files must be in this format:
- The first line should be a header containing the locales
- The first column should contain the keys for the localization strings (which are the English strings in this game)
- The following columns should contain the translations for each language

Note: Additional columns with the locales already existing in the game will be ignored (so you can keep the original translations in the file)

You can use [this template](https://docs.google.com/spreadsheets/d/159jjgwiimEFuYmiFUlIKS75MIr6kHpIYdULLQ4FOtv0/edit?usp=sharing) to translate and create your own custom language `.csv`, it already contains the localization keys and the original translations, so you just need to add new columns for the new languages. Just keep in mind it may become outdated with game updates.

#### Extracting The Most Recent Localizations
If you want to extract the most recent localizations from the game yourself, you can use [Godot PCK Explorer](https://github.com/DmitriySalnikov/GodotPCKExplorer) to extract the `.po` files from the `Pratfall.pck` (located in the game's installation folder). They are located in `data/localization`. Once you have extracted a `.po` file, you can use a converter such as [csv2po](https://docs.translatehouse.org/projects/translate-toolkit/en/latest/commands/csv2po.html) to convert it to `.csv`, and then you are ready to add new languages.
