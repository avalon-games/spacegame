using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class stores all the player data in the current game instance.
 * If new data is added, the contents of SaveFile,SavePlayerData,SaveAndLoad 
 * may have to be changed as well
 */
public static class PlayerData
{
	//player progress
	public static int maxHealth { get; set; }
	public static int currHealth { get; set; }
	public static int maxOxygen { get; set; }
	public static int currOxygen { get; set; }
	public static int currUnlockedLevel { get; set; }
	public static int currLevel { get; set; }
	public static float[] checkpoint { get; set; } //location of current checkpoint
	public static string[] saveFileNames { get; set; }
	public static float inGameTime { get; set; }

	//options settings
	public static float volume { get; set; }
}