using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayArea : MonoBehaviour {
	public static PlayArea Instance    { get; private set; }
	public        Rect     WorldBounds { get; private set; }
	public event Action    OnBoundsChanged;

	[SerializeField] private UIDocument    uiDocument;
	private                  VisualElement _topMenuBar;
	private                  Camera        _cam;

	private void Awake() {
		if (Instance != null) Destroy(gameObject);
		else Instance = this;
	}

	private void OnEnable() {
		var root = uiDocument.rootVisualElement;
		_topMenuBar = root.Q(className: UI.TopBarClass);
		_topMenuBar?.RegisterCallback<GeometryChangedEvent>(RecalculateBounds);
		_cam = Camera.main;
	}

	private void OnDisable() { _topMenuBar?.UnregisterCallback<GeometryChangedEvent>(RecalculateBounds); }

	private void RecalculateBounds(GeometryChangedEvent evt) {
		if (_topMenuBar == null) return;

		var uiHeightPixels = UIExtensions.PanelToScreenPixels(_topMenuBar);
		var screenY        = _cam.pixelHeight - uiHeightPixels;

		var bottomLeft = _cam.ScreenToWorldPoint(new Vector3(0,            0,       Mathf.Abs(_cam.transform.position.z)));
		var topRight   = _cam.ScreenToWorldPoint(new Vector3(Screen.width, screenY, Mathf.Abs(_cam.transform.position.z)));

		WorldBounds = new Rect(bottomLeft.x,
		                       bottomLeft.y,
		                       topRight.x - bottomLeft.x,
		                       topRight.y - bottomLeft.y);

		OnBoundsChanged?.Invoke();

		Debug.Log($"Play Area Updated: {WorldBounds}");
	}
}