using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Paddle : MonoBehaviour {
	public static Action DragRelease;

	private Camera      _cam;
	private Rigidbody2D _rb;
	private float       _maxBoundary;
	private float       _minBoundary;
	private bool        _isTwisting = false;


	private void OnEnable() { EnhancedTouchSupport.Enable(); }

	private void OnDisable() { EnhancedTouchSupport.Disable(); }

	void Awake() => _rb = GetComponent<Rigidbody2D>();

	private void Start() {
		_cam                              =  Camera.main;
		PlayArea.Instance.OnBoundsChanged += RecalculateBoundaries;
	}

	private void Update() {
		if (Touch.activeTouches.Count <= 0)
			return;
		var screenPosition = Touch.activeTouches[0].screenPosition;
		var worldPosition  = _cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
		var clampedX       = Mathf.Clamp(worldPosition.x, _minBoundary, _maxBoundary);
		var finalTargetPos = new Vector2(clampedX, transform.position.y);

		var smoothedPosition = Vector2.Lerp(_rb.position, finalTargetPos, Time.deltaTime * 500);
		transform.position = smoothedPosition;
		// _rb.MovePosition(smoothedPosition); // too chaotic. lets let the paddle teleport x wise

		if (Touch.activeTouches.Count > 1) {
			_isTwisting = true;

			var dragPosition           = Touch.activeTouches[1].screenPosition;
			var dragStartPosition      = Touch.activeTouches[1].startScreenPosition;
			var dragWorldPosition      = _cam.ScreenToWorldPoint(new Vector3(dragPosition.x,      dragPosition.y,      10f));
			var dragStartWorldPosition = _cam.ScreenToWorldPoint(new Vector3(dragStartPosition.x, dragStartPosition.y, 10f));

			_rb.MoveRotation(90 + (dragStartWorldPosition.y - dragWorldPosition.y) * 45);
		}

		else if (_isTwisting) {
			_isTwisting = false;
			DragRelease?.Invoke();
		}
	}

	private void RecalculateBoundaries() {
		var paddleHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
		_minBoundary = PlayArea.Instance.WorldBounds.xMin + paddleHalfWidth;
		_maxBoundary = PlayArea.Instance.WorldBounds.xMax - paddleHalfWidth;
	}
}