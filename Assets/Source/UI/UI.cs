using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
	public RectVariable playArea;

	static class Ids
	{
		public const string TopBar   = "top-bar";
		public const string ShopOpen = "shop-container--open";
		public const string Shop     = "shop-container";
		public const string MenuBtn  = "MenuButton";
	}

	VisualElement _shopContainer;
	VisualElement _topMenuBar;
	Button        _menuButton;
	Camera        _cam;

	void OnEnable()
	{
		var root = GetComponent<UIDocument>().rootVisualElement;

		_shopContainer = root.Q<VisualElement>(className: Ids.Shop);
		_menuButton    = root.Q<Button>(Ids.MenuBtn);
		_topMenuBar    = root.Q(className: Ids.TopBar);
		_cam           = Camera.main;

		_topMenuBar?.RegisterCallback<GeometryChangedEvent>(RecalculateBounds);
		_topMenuBar?.schedule.Execute(() => RecalculateBounds(null));

		if (_menuButton != null)
			_menuButton.clicked += ToggleShop;
	}

	void OnDisable()
	{
		_topMenuBar?.UnregisterCallback<GeometryChangedEvent>(RecalculateBounds);

		if (_menuButton != null)
			_menuButton.clicked -= ToggleShop;
	}

	void RecalculateBounds(GeometryChangedEvent evt)
	{
		if (_topMenuBar == null || _cam == null) return;

		if (!UIExtensions.TryPanelToScreenPixels(_topMenuBar, out var topMenuPx))
			return;

		var bottomTopMenu = _cam.pixelHeight - topMenuPx;
		var camZPos = Mathf.Abs(_cam.transform.localPosition.z);

		var bottomLeft = _cam.ScreenToWorldPoint(new Vector3(0f, 0f, camZPos));
		var topRight   = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, bottomTopMenu, camZPos));

		var newBounds = new Rect(
			bottomLeft.x,
			bottomLeft.y,
			topRight.x - bottomLeft.x,
			topRight.y - bottomLeft.y
		);

		playArea?.SetValue(newBounds);
	}

	void ToggleShop() => _shopContainer.ToggleInClassList(Ids.ShopOpen);
}