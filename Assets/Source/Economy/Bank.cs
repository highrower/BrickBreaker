using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/Bank")]
public class Bank : ScriptableObject, ISaveable {
	[SerializeField] string id = "Player_Bank";
	public           string ID => id;

	public double currentCoins;
	public double cumulativeCoins;

	public event Action<double> OnCoinsChanged;

	void OnEnable() {
		currentCoins    = 0;
		cumulativeCoins = 0;
	}

	public void Save(SaveData data) {
		data.totalCoins = currentCoins;
	}

	public void Load(SaveData data) {
		currentCoins = data.totalCoins;
		OnCoinsChanged?.Invoke(currentCoins);
	}

	public void AddCoins(double amount) {
		currentCoins    += amount;
		cumulativeCoins += amount;
		OnCoinsChanged?.Invoke(currentCoins);
	}

	public bool TrySpendCoins(double amount) {
		if (currentCoins < amount)
			return false;
		currentCoins -= amount;
		OnCoinsChanged?.Invoke(currentCoins);
		return true;
	}
}