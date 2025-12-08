using UnityEngine;
using UnityEngine.UIElements;

public class CurrencyView : MonoBehaviour {
	[SerializeField] Bank bank;

	Label _coinLabel;

	const string CoinLabelClass = "current-coins-label";

	void OnEnable() {
		var root = GetComponent<UIDocument>().rootVisualElement;

		_coinLabel = root.Q<Label>(className: CoinLabelClass);

		if (bank != null) {
			bank.OnCoinsChanged += UpdateDisplay;
			UpdateDisplay(bank.currentCoins);
		}
	}

	void OnDisable() {
		if (bank != null)
			bank.OnCoinsChanged -= UpdateDisplay;
	}

	void UpdateDisplay(int bankCurrentCoins) {
		if (_coinLabel != null)
			_coinLabel.text = bankCurrentCoins.ToString();
	}
}