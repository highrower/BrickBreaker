using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
	[SerializeField] VisualTreeAsset   nodeTemplate;
	[SerializeField] Bank              bank;
	[SerializeField] List<ShopUpgrade> upgrades;
	[SerializeField] SaveData data;

	UpgradeGraph _graph;

	void OnEnable()
	{
		var root = GetComponent<UIDocument>().rootVisualElement;
		_graph = root.Q<UpgradeGraph>();
		_graph?.Initialize(upgrades, nodeTemplate, SetupNode);
		if (bank != null) bank.OnCoinsChanged += OnCoinsChanged;
	}

	void SetupNode(VisualElement entry, ShopUpgrade upgrade)
	{
		entry.Q<Label>(className: "shop-upgrade-graph-item__name").text = upgrade.title;
		var buyBtn = entry.Q<Button>(className: "shop-upgrade-graph-item__buy");

		upgrade.OnStateChanged += OnItemStateChanged;
		buyBtn.clicked         += () => TryBuy(upgrade);

		RefreshEntry(entry);
	}

	void OnDisable()
	{
		if (bank != null) bank.OnCoinsChanged        -= OnCoinsChanged;
		foreach (var x in upgrades) x.OnStateChanged -= OnItemStateChanged;
	}

	void OnItemStateChanged(ShopUpgrade item) => RefreshAll();
	void OnCoinsChanged(double          _)    => RefreshAll();

	void RefreshAll()
	{
		if (_graph == null) return;

		foreach (var child in _graph.Children())
			RefreshEntry(child);
	}

	void TryBuy(ShopUpgrade upg)
	{
		if (!upg.IsMaxed(data) && IsUnlocked(upg) && bank.TrySpend(upg.GetCost(data)))
			upg.ApplyUpgrade(data);
	}

	bool IsUnlocked(ShopUpgrade item) => item.prereqs.All(x => x.GetLevel(data) > 0);

	void RefreshEntry(VisualElement item)
	{
		var shopItem = (ShopUpgrade)item.userData;

		if (shopItem == null) return;

		var buyBtn = item.Q<Button>(className: "shop-upgrade-graph-item__buy");

		buyBtn.text = shopItem.IsMaxed(data) ? "MAX" :
					  !IsUnlocked(shopItem) ? "LOCKED" :
					  shopItem.GetCost(data).ToString();

		var canAfford = bank.currentCoins >= shopItem.GetCost(data);
		buyBtn.SetEnabled(!shopItem.IsMaxed(data) && IsUnlocked(shopItem) && canAfford);
	}
}