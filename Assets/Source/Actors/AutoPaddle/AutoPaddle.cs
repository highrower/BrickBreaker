using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class AutoPaddle : MonoBehaviour
{
	[SerializeField] RectReference bounds;
	[SerializeField] Bank          bank;
	[SerializeField] float speed;

	Rigidbody2D _rb;
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
		_halfWidth  = GetComponent<SpriteRenderer>().bounds.extents.x;
	}

	void Update()
	{
		var targetX = GetTargetX(bounds.Value, _halfWidth);
		var finalTargetPos = new Vector2(targetX, transform.position.y);
		var smoothedPosition = Vector2.Lerp(_rb.position, finalTargetPos, Time.deltaTime * 500);
		transform.position = smoothedPosition;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Drop"))
			CollectDrop(other);
	}

	float GetTargetX(Rect bound, float xOffset)
	{
		var xMin = bound.min.x + xOffset;
		var xMax = bound.max.x - xOffset;

		var phase = Time.time * speed * (Mathf.PI * 2f);
		var s = Mathf.Sin(phase);          // [-1, 1]
		var t = (s + 1f) * 0.5f;           // [0, 1]

		return Mathf.Lerp(xMin, xMax, t);
	}

	void CollectDrop(Collider2D drop)
	{
		if (bank)
			bank.AddCoins(drop.GetComponent<Drop>().Value);

		drop.GetComponent<Drop>().Deactivate();
	}
}