using Godot;
using System;

public static class Logger
{
	private const string MOD_DEBUG_PREFIX = "LOC_MOD: ";
	
	public static void Print(string message)
	{
		GD.Print($"{MOD_DEBUG_PREFIX}{message}");
	}
}
