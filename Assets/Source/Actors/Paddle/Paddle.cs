using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Paddle : MonoBehaviour {
	public Action DragRelease;

	[SerializeField] RectReference bounds;
	[SerializeField] Bank          bank;

	Camera       _cam;
	Rigidbody2D  _rb;
	IPaddleInput _input;
	bool         _isTwisting;
	float        _halfWidth;


	void OnEnable() {
		bank.ResetBank();
		EnhancedTouchSupport.Enable();
	}

	void OnDisable() { EnhancedTouchSupport.Disable(); }

	void Awake() {
		_rb = GetComponent<Rigidbody2D>();
#if UNITY_EDITOR
		_input = new EditorPaddleInput();
#else
		_input = new MobilePaddleInput();
#endif
	}

	void Start() {
		_cam       = Camera.main;
		_halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
	}

	void Update() {
		var targetX          = _input.GetTargetX(_cam, bounds.Value, _halfWidth);
		var finalTargetPos   = new Vector2(targetX, transform.position.y);
		var smoothedPosition = Vector2.Lerp(_rb.position, finalTargetPos, Time.deltaTime * 500);
		transform.position = smoothedPosition;

		if (_input.IsTwisting(_cam, out float targetAngle)) {
			_isTwisting = true;
			_rb.MoveRotation(targetAngle);
		}
		else if (_isTwisting) {
			DragRelease?.Invoke();
			_isTwisting = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Drop"))
			CollectDrop(other);
	}

	void CollectDrop(Collider2D drop) {
		if (bank)
			bank.AddCoins(drop.GetComponent<Drop>().Value);
		drop.GetComponent<Drop>().Deactivate();
	}
}