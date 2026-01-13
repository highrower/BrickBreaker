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
		if (_topMenuBar == null) return;

		var bottomTopMenu = _cam.pixelHeight - UIExtensions.PanelToScreenPixels(_topMenuBar);
		var camZPos = Mathf.Abs(_cam.transform.localPosition.z);
		var bottomLeft = _cam.ScreenToWorldPoint(new Vector3(0, 0, camZPos));
		var topRight = _cam.ScreenToWorldPoint(new Vector3(Screen.width, bottomTopMenu, camZPos));

		var newBounds = new Rect(bottomLeft.x,
								 bottomLeft.y,
								 topRight.x - bottomLeft.x,
								 topRight.y - bottomLeft.y);

		if (playArea != null)
			playArea.SetValue(newBounds);
	}

	void ToggleShop() => _shopContainer.ToggleInClassList(Ids.ShopOpen);
}