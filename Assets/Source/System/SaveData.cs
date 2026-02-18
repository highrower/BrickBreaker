using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class SaveData : ISerializationCallbackReceiver
{
	[NonSerialized]
	public Dictionary<string, int> UpgradeIdToLevel = new();

	public double totalCoins;

	[SerializeField]
	List<UpgradeRecord> upgradeList = new();

	public void OnBeforeSerialize()
	{
		upgradeList.Clear();

		foreach (var kvp in UpgradeIdToLevel)
			upgradeList.Add(new UpgradeRecord { id = kvp.Key, level = kvp.Value });
	}

	public void OnAfterDeserialize()
	{
		UpgradeIdToLevel.Clear();

		foreach (var record in
				 upgradeList.Where(record => !UpgradeIdToLevel.ContainsKey(record.id)))
			UpgradeIdToLevel.Add(record.id, record.level);
	}
}

[Serializable]
public struct UpgradeRecord
{
	public string id;
	public int    level;
}