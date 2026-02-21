using System;
using UnityEngine;

public class AutoPaddle : MonoBehaviour
{
	[SerializeField] RectReference bounds;
	[SerializeField] Bank          bank;
	[SerializeField] float speed;
	[SerializeField] SaveData progress;

	Rigidbody2D _rb;
	SpriteRenderer _renderer;
	CapsuleCollider2D _collider;
	bool        _isTwisting;
	float       _halfWidth;
	
	Action _onUnlockedHandler;



	void Awake()
	{
		_onUnlockedHandler = () => ToggleAutoPaddle(AutoPaddleProgress.GetUnlocked(progress));
		_rb = GetComponent<Rigidbody2D>();
		_renderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<CapsuleCollider2D>();
	}
	
	void OnEnable()
	{
		if (progress)
			AutoPaddleProgress.OnUnlocked +=  _onUnlockedHandler;
	}

	void OnDisable()
	{
		if (progress)
			AutoPaddleProgress.OnUnlocked -=  _onUnlockedHandler;
	}

	void Start()
	{
		_halfWidth  = _renderer.bounds.extents.x;
		ToggleAutoPaddle(progress && AutoPaddleProgress.GetUnlocked(progress));
	}

	void Update()
	{
		if (!AutoPaddleProgress.GetUnlocked(progress)) return;
		
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

	void ToggleAutoPaddle(bool enable)
	{
		_renderer.enabled=enable; 
		_collider.enabled=enable; 
		if (_rb) _rb.simulated = enable;
	}

	float GetTargetX(Rect bound, float xOffset)
	{
		var xMin = bound.min.x + xOffset;
		var xMax = bound.max.x - xOffset;

		var phase = Time.time * speed * (Mathf.PI * 2f);
		var s = Mathf.Sin(phase);
		var t = (s + 1f) * 0.5f;

		return Mathf.Lerp(xMin, xMax, t);
	}

	void CollectDrop(Collider2D drop)
	{
		if (bank)
			bank.AddCoins(drop.GetComponent<Drop>().Value);

		drop.GetComponent<Drop>().Deactivate();
	}
}