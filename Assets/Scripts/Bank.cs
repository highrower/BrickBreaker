using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/Bank")]
public class Bank : ScriptableObject {
	public int currentCoins;
	public int cumulativeCoins;

	public event Action<int> OnCoinsChanged;

	public void AddCoins(int amount) {
		currentCoins    += amount;
		cumulativeCoins += amount;
		OnCoinsChanged?.Invoke(currentCoins);
	}

	public bool SpendCoins(int amount) {
		if (currentCoins < amount)
			return false;
		currentCoins -= amount;
		OnCoinsChanged?.Invoke(currentCoins);
		return true;
	}

	public void ResetBank() {
		currentCoins    = 0;
		cumulativeCoins = 0;
		OnCoinsChanged?.Invoke(currentCoins);
	}
}