using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Microsoft.VisualBasic.FileIO;

public static class ModEntry
{
	public static void ModInit()
	{
		ModUtils.Print("Initializing Localization Mod...");
		LocalizationManager.Init();
	}

	public static void ModDestroy()
	{
		ModUtils.Print("Localization Mod destroyed!");
	}
}
