using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Paddle : MonoBehaviour
{
	public Action DragRelease;

	[SerializeField] RectReference bounds;
	[SerializeField] Bank          bank;

	Camera      _cam;
	Rigidbody2D _rb;
	PaddleView  _paddleView;
	Collider2D  _collider;
	bool        _isTwisting;
	float       _halfWidth;

	void OnEnable() { EnhancedTouchSupport.Enable(); }

	void OnDisable() { EnhancedTouchSupport.Disable(); }

	void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		_cam        = Camera.main;
		_halfWidth  = GetComponent<SpriteRenderer>().bounds.extents.x;
		_paddleView = GetComponent<PaddleView>();
		_collider   = GetComponent<Collider2D>();
	}

	void Update()
	{
		var targetX = PaddleInput.GetTargetX(_cam, bounds.Value, _halfWidth, transform.position.x);
		var finalTargetPos = new Vector2(targetX, transform.position.y);
		var smoothedPosition = Vector2.Lerp(_rb.position, finalTargetPos, Time.deltaTime * 500);
		transform.position = smoothedPosition;
		HandleTwist();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Drop"))
			CollectDrop(other);
	}

	void HandleTwist()
	{
		if (PaddleInput.IsTwisting(_cam, out var targetAngle))
		{
			_isTwisting = true;
			_rb.MoveRotation(targetAngle);
		}
		else if (_isTwisting)
		{
			DragRelease?.Invoke();
			_isTwisting = false;
		}

		_collider.isTrigger = _isTwisting;
		_paddleView.SetTwistView(_isTwisting);
	}

	void CollectDrop(Collider2D drop)
	{
		if (bank)
			bank.AddCoins(drop.GetComponent<Drop>().Value);

		drop.GetComponent<Drop>().Deactivate();
	}
}