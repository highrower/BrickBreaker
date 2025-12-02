using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour {
	public const string TopBarClass = "top-bar";

	private static class Ids {
		public const string ShopOpen = "shop-container--open";
		public const string Shop     = "shop-container";
		public const string MenuBtn  = "MenuButton";
	}

	private VisualElement _shopContainer;
	private Button        _menuButton;

	private void OnEnable() {
		var root = GetComponent<UIDocument>().rootVisualElement;

		_shopContainer = root.Q<VisualElement>(className: Ids.Shop);
		_menuButton    = root.Q<Button>(Ids.MenuBtn);

		if (_menuButton != null)
			_menuButton.clicked += ToggleShop;
	}

	private void OnDisable() {
		if (_menuButton != null)
			_menuButton.clicked -= ToggleShop;
	}

	private void ToggleShop() { _shopContainer.ToggleInClassList(Ids.ShopOpen); }
}