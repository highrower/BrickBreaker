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

	public abstract int CurrLvl { get; }

	public bool IsMaxed => CurrLvl >= maxLevel;

	public event Action<ShopUpgrade> OnStateChanged;

	public void ApplyUpgrade()
	{
		if (IsMaxed) return;

		UpgradeLogic();
		OnStateChanged?.Invoke(this);
	}

	protected abstract void UpgradeLogic();

	public abstract int GetCost();
}