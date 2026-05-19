

using System.IO;
using System.Text.Json;
using Godot;

public static class Config
{
	private static string CONFIG_FILE_PATH => Path.Combine(ModUtils.MOD_PATH, "config.json");
	public static ConfigData Data = new();
	public static void Load()
	{
		if (Engine.IsEditorHint())
			return;

		string jsonPath = CONFIG_FILE_PATH;
		if (!File.Exists(jsonPath))
			return;

		string json = File.ReadAllText(jsonPath);
		ConfigData loadedConfig = JsonSerializer.Deserialize<ConfigData>(json);
		if (loadedConfig != null)
			Data = loadedConfig;
	}

	public static void Save()
	{
		if (Engine.IsEditorHint())
			return;

		string json = JsonSerializer.Serialize(Data);
		File.WriteAllText(CONFIG_FILE_PATH, json);
	} 
}
	