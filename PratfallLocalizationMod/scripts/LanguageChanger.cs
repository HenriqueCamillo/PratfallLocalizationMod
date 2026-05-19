using Godot;

public partial class LanguageChanger : Node
{
	private const int CHANGES_TO_SWITCH_TO_CUSTOM = 3;
	private static int _languageChanges;
	private static bool _reapplyCustomLanguage = true; // The game sets the langauge after the mod

	public override void _Notification(int what)
	{
		if (what == (int)NotificationTranslationChanged)
		{
			if (LocalizationManager.HasJustSetLanguage)
			{
				LocalizationManager.HasJustSetLanguage = false;
				return;
			}

			if (_reapplyCustomLanguage)
			{
				LocalizationManager.TrySetInitialLocale();
				_reapplyCustomLanguage = false;
			}

			LocalizationManager.IsCustomLanguageActive = false;
			_languageChanges++;
			
			if (_languageChanges >= CHANGES_TO_SWITCH_TO_CUSTOM)
			{
				_languageChanges = 0;
				LocalizationManager.SetLanguageToNextLocale();
			}
			 
			LocalizationManager.UpdateLastSelectedLocale();
		}
	}
}
