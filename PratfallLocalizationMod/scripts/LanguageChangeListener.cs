using Godot;

public partial class LanguageChangeListener : Node
{
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
				LocalizationManager.TrySetInitialLanguage();
				_reapplyCustomLanguage = false;
				return;
			}

			LocalizationManager.HandleLanguageChange();
		}
	}
}
