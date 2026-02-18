using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
	const string SaveFileName = "gamedata.json";

	[SerializeField] List<ScriptableObject> objectsToSave;

	static string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

	void Awake() { LoadGame(); }

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
		var data = new SaveData();
		objectsToSave.ForEach(obj => (obj as ISaveable)?.Save(data));

		var json = JsonUtility.ToJson(data, true);
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

		var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
		objectsToSave.ForEach(obj => (obj as ISaveable)?.Load(data));
	}
}