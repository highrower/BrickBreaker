using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour {
	const            string        ClassTopBar         = "top-bar";
	const            string        ClassButtonDisabled = "top-bar__button--disabled";
	[SerializeField] GameObject    topWall;
	private          VisualElement _shopContainer;
	private          Button        _menuButton;
	private          VisualElement _topMenuBar;

	void OnEnable() {
		var uiDoc = GetComponent<UIDocument>();
		var root  = uiDoc.rootVisualElement;

		_shopContainer      =  root.Q<VisualElement>(className: "shop-container");
		_menuButton         =  root.Q<Button>("MenuButton");
		_menuButton.clicked += ToggleShop;

		_topMenuBar = root.Q(className: "top-bar");

		if (_topMenuBar != null) {
			_topMenuBar.RegisterCallback<GeometryChangedEvent>(AlignWallToUI);
		}
	}

	private void AlignWallToUI(GeometryChangedEvent evt) {
		if (topWall == null) return;

		var uiHeightPanelUnits = _topMenuBar.resolvedStyle.height;
		var screenZero         = new Vector2(0, 0);
		var screenHundred      = new Vector2(0, 100);
		var panelZero          = RuntimePanelUtils.ScreenToPanel(_topMenuBar.panel, screenZero);
		var panelHundred       = RuntimePanelUtils.ScreenToPanel(_topMenuBar.panel, screenHundred);
		var panelDist          = Mathf.Abs(panelHundred.y - panelZero.y);
		var pixelsPerUnit      = 100f               / panelDist;
		var uiHeightPixels     = uiHeightPanelUnits * pixelsPerUnit;
		var cutLinePixelY      = Camera.main.pixelHeight - uiHeightPixels;
		var distFromCam        = Mathf.Abs(Camera.main.transform.position.z - topWall.transform.position.z);
		var cutLineWorldPos    = Camera.main.ScreenToWorldPoint(new Vector3(0, cutLinePixelY, distFromCam));
		var wallCollider       = topWall.GetComponent<Collider2D>();
		var halfHeightWorld    = wallCollider.bounds.extents.y;

		topWall.transform.position = new Vector3(topWall.transform.position.x,
		                                         cutLineWorldPos.y + halfHeightWorld,
		                                         topWall.transform.position.z);

		Debug.Log($"MATH FIXED: UI Units: {uiHeightPanelUnits} | Ratio: {pixelsPerUnit} | Real Pixels: {uiHeightPixels}");
	}

	private void ToggleShop() {
		if (_shopContainer.ClassListContains("shop-container--open"))
			_shopContainer.RemoveFromClassList("shop-container--open");
		else
			_shopContainer.AddToClassList("shop-container--open");
	}
}