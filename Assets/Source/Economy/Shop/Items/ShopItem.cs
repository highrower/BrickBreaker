using UnityEngine;

public abstract class ShopItem : ScriptableObject {
	public            string title;
	[TextArea] public string description;
	public            int    baseCost;
	public            int    maxLevel = 5;

	protected abstract int CurrentLevel { get; }

	public bool IsMaxed => CurrentLevel >= maxLevel;

	public abstract void ApplyUpgrade();

	public abstract int GetCost();
}