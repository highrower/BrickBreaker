using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
	const string SaveFileName = "gamedata.json";
	SaveData _data;


	static string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

	void Awake()
	{
		_data = GetComponent<SaveData>();  
		LoadGame();                       
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus) SaveGame();
	}
	
	void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus)
			SaveGame();
	}

	void OnApplicationQuit() { SaveGame(); }

	[ContextMenu("Force Save")]
	public void SaveGame()
	{
		var json = JsonUtility.ToJson(_data, true);
		File.WriteAllText(SavePath, json);
	}

	[ContextMenu("Force Load")]
	public void LoadGame()
	{
		if (!File.Exists(SavePath))
		{
			Debug.Log("No save file found. Starting New Game.");
			return;
		}
		var json = File.ReadAllText(SavePath);
		JsonUtility.FromJsonOverwrite(json, _data);
	}
}