using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour {
	[SerializeField] VisualTreeAsset shopEntryTemplate;
	[SerializeField] Bank            bank;
	[SerializeField] List<ShopItem>  availableUpgrades;

	readonly List<VisualElement> _shopItems = new();

	void OnEnable() {
		var root          = GetComponent<UIDocument>().rootVisualElement;
		var listContainer = root.Q<VisualElement>("ShopListContainer");

		listContainer.Clear();
		_shopItems.Clear();

		if (bank != null) bank.OnCoinsChanged += OnCoinsChanged;

		foreach (var item in availableUpgrades) {
			var entry = shopEntryTemplate.Instantiate();

			entry.Q<Label>(className: "shop-item__name").text = item.title;
			var buyBtn = entry.Q<Button>(className: "shop-item__buy");

			entry.userData = item;

			RefreshEntry(entry);

			item.OnStateChanged += OnItemStateChanged;
			buyBtn.clicked      += () => TryBuy(item);
			listContainer.Add(entry);
			_shopItems.Add(entry);
		}
	}

	void OnDisable() {
		if (bank != null) bank.OnCoinsChanged -= OnCoinsChanged;
		foreach (var item in availableUpgrades)
			item.OnStateChanged -= OnItemStateChanged;
	}

	void OnItemStateChanged(ShopItem item) => RefreshAll();

	void OnCoinsChanged(double _) => RefreshAll();

	void RefreshAll() => _shopItems.ForEach(RefreshEntry);

	void TryBuy(ShopItem item) {
		if (!item.IsMaxed && bank.TrySpendCoins(item.GetCost()))
			item.ApplyUpgrade();
	}

	void RefreshEntry(VisualElement item) {
		var shopItem  = (ShopItem)item.userData;
		var buyBtn    = item.Q<Button>(className: "shop-item__buy");
		var costLabel = item.Q<Label>(className: "shop-item__cost");

		if (shopItem.IsMaxed) {
			costLabel.text = "MAX";
			buyBtn.SetEnabled(false);
		}
		else {
			costLabel.text = shopItem.GetCost().ToString();
			buyBtn.SetEnabled(bank.currentCoins >= shopItem.GetCost());
		}
	}
}