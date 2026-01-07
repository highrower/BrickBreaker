using System;
using UnityEngine;

public abstract class ShopItem : ScriptableObject {
	public            string title;
	[TextArea] public string description;
	public            int    baseCost;
	public            int    maxLevel = 5;

	protected abstract int CurrentLevel { get; }

	public bool IsMaxed => CurrentLevel >= maxLevel;

	public event Action<ShopItem> OnStateChanged;

	public void ApplyUpgrade() {
		if (IsMaxed)
			return;

		UpgradeLogic();
		OnStateChanged?.Invoke(this);
	}

	protected abstract void UpgradeLogic();

	public abstract int GetCost();
}