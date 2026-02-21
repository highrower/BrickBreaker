using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class SaveData : MonoBehaviour, ISerializationCallbackReceiver
{
	
	[NonSerialized]
	public Dictionary<string, int> UpgradeIdToLevel = new();

	public double totalCoins;

	[SerializeField]
	List<UpgradeRecord> _upgradeList = new();

	public int GetInt(string key, int defaultValue = 0)
		=> UpgradeIdToLevel.GetValueOrDefault(key, defaultValue);

	public void SetInt(string key, int value, Action a = null)
	{
		UpgradeIdToLevel[key] = value;
		a?.Invoke();
	}

	public bool GetBool01(string key, bool defaultValue = false)
		=> GetInt(key, defaultValue ? 1 : 0) == 1;

	public void SetBool01(string key, bool value, Action a = null)
		=> SetInt(key, value ? 1 : 0, a);
	
	
	public void OnBeforeSerialize()
	{
		_upgradeList.Clear();

		foreach (var kvp in UpgradeIdToLevel)
			_upgradeList.Add(new UpgradeRecord { id = kvp.Key, level = kvp.Value });
	}

	public void OnAfterDeserialize()
	{
		UpgradeIdToLevel.Clear();

		foreach (var record in
				 _upgradeList.Where(record => !UpgradeIdToLevel.ContainsKey(record.id)))
			UpgradeIdToLevel.Add(record.id, record.level);
	}
}

[Serializable]
public struct UpgradeRecord
{
	public string id;
	public int    level;
}