using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour {
	public RectVariable playArea;

	private static class Ids {
		public const string TopBar   = "top-bar";
		public const string ShopOpen = "shop-container--open";
		public const string Shop     = "shop-container";
		public const string MenuBtn  = "MenuButton";
	}

	private VisualElement _shopContainer;
	private VisualElement _topMenuBar;
	private Button        _menuButton;
	private Camera        _cam;

	private void OnEnable() {
		var root = GetComponent<UIDocument>().rootVisualElement;

		_shopContainer = root.Q<VisualElement>(className: Ids.Shop);
		_menuButton    = root.Q<Button>(Ids.MenuBtn);
		_topMenuBar    = root.Q(className: Ids.TopBar);
		_cam           = Camera.main;

		_topMenuBar?.RegisterCallback<GeometryChangedEvent>(RecalculateBounds);
		if (_menuButton != null)
			_menuButton.clicked += ToggleShop;
	}

	private void OnDisable() {
		_topMenuBar?.UnregisterCallback<GeometryChangedEvent>(RecalculateBounds);
		if (_menuButton != null)
			_menuButton.clicked -= ToggleShop;
	}

	private void RecalculateBounds(GeometryChangedEvent evt) {
		if (_topMenuBar == null) return;

		var bottomTopMenu = _cam.pixelHeight - UIExtensions.PanelToScreenPixels(_topMenuBar);
		var bottomLeft    = _cam.ScreenToWorldPoint(new Vector3(0,            0,             Mathf.Abs(_cam.transform.position.z)));
		var topRight      = _cam.ScreenToWorldPoint(new Vector3(Screen.width, bottomTopMenu, Mathf.Abs(_cam.transform.position.z)));

		var newBounds = new Rect(bottomLeft.x,
		                         bottomLeft.y,
		                         topRight.x - bottomLeft.x,
		                         topRight.y - bottomLeft.y);

		if (playArea != null)
			playArea.SetValue(newBounds);
	}

	private void ToggleShop() => _shopContainer.ToggleInClassList(Ids.ShopOpen);
}