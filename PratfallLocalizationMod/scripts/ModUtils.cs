using Godot;
using System.IO;

public static class ModUtils
{
	public static string GAME_PATH => OS.GetExecutablePath().GetBaseDir();
	public static string MOD_PATH => Path.Combine(GAME_PATH, "mods/PratfallLocalizationMod/");
	private const string MOD_DEBUG_PREFIX = "LOC_MOD: ";
    
    public static void Print(string message)
	{
		GD.Print($"{MOD_DEBUG_PREFIX}{message}");
	}

	public static void PrintErr(string message)
	{
		GD.PrintErr($"{MOD_DEBUG_PREFIX}{message}");
	}
}
