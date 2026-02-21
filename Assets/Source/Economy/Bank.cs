using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/Bank")]
public class Bank : ScriptableObject
{
	[SerializeField] string id = "Player_Bank";
	public           string ID => id;

	public double currentCoins;

	public event Action<double> OnCoinsChanged;

	public void Save(SaveData data) => data.totalCoins = currentCoins;

	public void Load(SaveData data)
	{
		currentCoins = data.totalCoins;
		OnCoinsChanged?.Invoke(currentCoins);
	}

	public void AddCoins(double amount)
	{
		currentCoins += amount;
		OnCoinsChanged?.Invoke(currentCoins);
	}

	public bool TrySpend(double amount)
	{
		if (currentCoins < amount) return false;

		currentCoins -= amount;
		OnCoinsChanged?.Invoke(currentCoins);

		return true;
	}
}