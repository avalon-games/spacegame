using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
	//player progress
	public static int maxHealth { get; set; }
	public static int currHealth { get; set; }
	public static int maxOxygen { get; set; }
	public static int currOxygen { get; set; }
	public static int currUnlockedLevel { get; set; }
	public static int currLevel { get; set; }

	//options settings
	public static float volume { get; set; }
}