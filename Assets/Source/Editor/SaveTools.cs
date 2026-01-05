using UnityEditor;
using UnityEngine;
using System.IO;

public static class SaveTools {
	[MenuItem("Tools/Reset Save File")]
	public static void DeleteSaveFile() {
		var path = Path.Combine(Application.persistentDataPath, "gamedata.json");
		Debug.Log($"<color=red>Deleting Save File at: {path}</color>");
		File.Delete(path);
	}

	[MenuItem("Tools/Open Save File Folder")]
	public static void OpenSaveLocation() {
		var path = Application.persistentDataPath;
		EditorUtility.RevealInFinder(path);
	}
}