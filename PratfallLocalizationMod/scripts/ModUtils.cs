using System;
using System.Collections;
using Godot;

public static class ModUtils
{
	private const string MOD_DEBUG_PREFIX = "LOC_MOD: ";
	private const string MOD_NAME = "Pratfall Localization Mod";
	private static string _modPath;
	public static string MOD_PATH => _modPath;

	public static void Init()
	{
		FetchModPath();
	}

	private static void FetchModPath()
	{
		foreach (ModManifest mod in ModManager.Mods)
		{
			if (mod.Name == MOD_NAME)
			{
				_modPath = mod.Directory;
				break;
			}
		}
	}
	
	public static void Print(string message)
	{
		GD.Print($"{MOD_DEBUG_PREFIX}{message}");
	}

	public static void PrintErr(string message)
	{
		GD.PrintErr($"{MOD_DEBUG_PREFIX}{message}");
	}
}
