using Godot;

public partial class LanguageChanger : Node
{
	private const int CHANGES_TO_SWITCH_TO_CUSTOM = 3;
	private static int _languageChanges;
	private static bool _ignoreNextNotification = true;

	public override void _Notification(int what)
	{
		if (what == (int)NotificationTranslationChanged)
		{
			if (_ignoreNextNotification)
			{
				_ignoreNextNotification = false;
				return;
			}

			LocalizationManager.IsCustomLanguageActive = false;
			_languageChanges++;
			
			if (_languageChanges >= CHANGES_TO_SWITCH_TO_CUSTOM)
			{
				_ignoreNextNotification = true;
				_languageChanges = 0;
				LocalizationManager.SetLanguageToNextLocale();
			}
		}
	}
}
