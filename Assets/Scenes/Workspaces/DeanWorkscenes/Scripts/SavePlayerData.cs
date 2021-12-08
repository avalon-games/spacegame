using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * This class acts as the middleman between PlayerData and SaveFile as PlayerData cannot be Serializable
 */
[System.Serializable]
public class SavePlayerData
{
	public int maxHealth;
	public int currHealth;
	public int maxOxygen;
	public int currOxygen;
	public int currUnlockedLevel;
	public int currLevel;
	public float[] checkpoint;

	public SavePlayerData() {
		maxHealth = PlayerData.maxHealth;
		currHealth = PlayerData.currHealth;
		maxOxygen = PlayerData.maxOxygen;
		currOxygen = PlayerData.currOxygen;
		currUnlockedLevel = PlayerData.currUnlockedLevel;
		currLevel = PlayerData.currLevel;
		checkpoint = PlayerData.checkpoint;
	}
}
