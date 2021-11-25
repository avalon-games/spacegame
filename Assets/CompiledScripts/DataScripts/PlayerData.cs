using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
	public static int maxHealth { get; set; }
	public static int currHealth { get; set; }
	public static float maxOxygen { get; set; }
	public static float currOxygen { get; set; }
	public static int currUnlockedLevel { get; set; }
	public static int currLevel { get; set; }

}
//caches player data across scenes, for long term storage, this data is moved into save files
//other classes call to access this script and write to it
//healthbar can set get all children objects and put in list on start, when required, disable sprite to indicate decrease health