using Godot;

public partial class LanguageChanger : Node
{
	private const int CHANGES_TO_SWITCH_TO_CUSTOM = 4;
	private static int _languageChanges;

	public override void _Notification(int what)
	{
		if (what == (int)NotificationTranslationChanged)
		{
			_languageChanges++;
			if (_languageChanges >= CHANGES_TO_SWITCH_TO_CUSTOM)
			{
				_languageChanges = 0;
				LocalizationManager.SetLanguageToNextLocale();
			}
		}
	}
}
