using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/**
 * This class saves the game data into file
 * 
 * Tutorial used: https://youtu.be/XOjd_qU2Ido
 */
public static class SaveFile
{
	public static void Save(int i) { //saves to "i"th savefile
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/savefile" + i.ToString() + ".save";

		FileStream stream = new FileStream(path, FileMode.Create);
		SavePlayerData data = new SavePlayerData();

		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static void Load(int i) { //load the "i"th savefile
		string path = Application.persistentDataPath + "/savefile" + i.ToString() + ".save";
		if (File.Exists(path)) {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			SavePlayerData data = formatter.Deserialize(stream) as SavePlayerData;

			//load data back into static data class
			PlayerData.maxHealth = data.maxHealth;
			PlayerData.currHealth = data.currHealth;
			PlayerData.maxOxygen = data.maxOxygen;
			PlayerData.currOxygen = data.currOxygen;
			PlayerData.currUnlockedLevel = data.currUnlockedLevel;
			PlayerData.currLevel = data.currLevel;
			PlayerData.checkpoint = data.checkpoint;
			Debug.Log("data loaded");

			stream.Close();
		} else {
			Debug.LogError("ERROR *** savefile not found in: " + path);		}
	}
}
