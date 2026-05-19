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
		Type modManagerType = null;
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			modManagerType = assembly.GetType("ModManager");
			if (modManagerType != null) 
				break;
		}

		var modsProperty = modManagerType.GetProperty("Mods");
		var modsList = modsProperty.GetValue(null) as IEnumerable;
		foreach (var mod in modsList)
		{
			var nameProperty = mod.GetType().GetProperty("Name");
       		string modName = nameProperty?.GetValue(mod) as string;

			Print(modName);
			if (modName == MOD_NAME)
			{
				var dirProperty = mod.GetType().GetProperty("Directory");
            	_modPath = dirProperty?.GetValue(mod) as string;
				
				Print(_modPath);
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
