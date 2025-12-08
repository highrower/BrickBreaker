using UnityEngine;

public static class GameProgress {
	const string DamageLevelKey = "BallDamageLevel";

	public static int DamageLevel {
		get => PlayerPrefs.GetInt(DamageLevelKey, 0);
		set {
			PlayerPrefs.SetInt(DamageLevelKey, value);
			PlayerPrefs.Save();
		}
	}

	public static void ResetAllData() {
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}