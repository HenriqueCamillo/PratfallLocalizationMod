using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Microsoft.VisualBasic.FileIO;

public static class ModEntry
{
	public static void ModInit()
	{
		ModUtils.Print("Initializing Localization Mod...");
		InitAfterOneFrame();
	}
	
	private async static Task InitAfterOneFrame()
	{
		var mainLoop = Engine.GetMainLoop();
		if (mainLoop is SceneTree tree)
		  	await tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);

		ReallyInit();
	}

	private static void ReallyInit()
	{
		ModUtils.Print("Really Initializing Localization Mod...");
		ModUtils.Init();
		LocalizationManager.Init();
	}

	public static void ModDestroy()
	{
		ModUtils.Print("Localization Mod destroyed!");
	}
}
