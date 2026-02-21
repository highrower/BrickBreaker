using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopUpgrade : ScriptableObject
{
	public            string            title;
	[TextArea] public string            description;
	public            int               baseCost;
	public            int               maxLevel = 5;
	public            string            id;
	public            Sprite            icon;
	public            Vector2           gridPosition;
	public            List<ShopUpgrade> prereqs = new();

	public abstract int GetLevel(SaveData data);
	public bool IsMaxed(SaveData data) => GetLevel(data) > maxLevel;

	public event Action<ShopUpgrade> OnStateChanged;

	public void ApplyUpgrade(SaveData data)
	{
		if (IsMaxed(data)) return;

		UpgradeLogic(data);
		OnStateChanged?.Invoke(this);
	}

	protected abstract void UpgradeLogic(SaveData data);

	public abstract int GetCost(SaveData data);
}