using UnityEngine;
using UnityEngine.UIElements;

public class CurrencyView : MonoBehaviour {
	[SerializeField] private Bank bank;

	private Label _coinLabel;

	private const string CoinLabelClass = "current-coins-label";

	private void OnEnable() {
		var root = GetComponent<UIDocument>().rootVisualElement;

		_coinLabel = root.Q<Label>(className: CoinLabelClass);

		if (bank != null) {
			bank.OnCoinsChanged += UpdateDisplay;
			UpdateDisplay(bank.currentCoins);
		}
	}

	private void OnDisable() {
		if (bank != null)
			bank.OnCoinsChanged -= UpdateDisplay;
	}

	private void UpdateDisplay(int bankCurrentCoins) {
		if (_coinLabel != null)
			_coinLabel.text = bankCurrentCoins.ToString();
	}
}