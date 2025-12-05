using UnityEngine;

public class BallBustThrough : MonoBehaviour {
	[SerializeField] private LayerMask    brickLayer;
	[SerializeField] private BallSettings settings;

	private Rigidbody2D      _rb;
	private CircleCollider2D _collider;
	private Brick            _brick;
	private Ball             _ball;


	private void Awake() {
		_rb       = GetComponent<Rigidbody2D>();
		_collider = GetComponent<CircleCollider2D>();
		_ball     = GetComponent<Ball>();
	}

	private void FixedUpdate() {
		if (!_brick)
			return;
		CheckForFutureBricks();
	}

	private void CheckForFutureBricks() {
		if (_rb.linearVelocity.magnitude < settings.speedThreshold) return;

		var hit = Physics2D.CircleCast(transform.position,
		                               _collider.radius,
		                               _rb.linearVelocity.normalized,
		                               settings.lookAheadDistance,
		                               brickLayer);

		if (!hit.collider)
			return;
		if (hit.collider.TryGetComponent<Brick>(out var brick))
			if (!(brick.CurrentHealth < _ball.CurrentDamage))
				return;
		_brick = brick;
		brick.SetIsTrigger(true);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<Brick>(out var brick) && brick == _brick)
			BreakThrough();
	}

	private void BreakThrough() {
		_rb.linearVelocity *= settings.bustThroughResistance / _brick.CurrentHealth;
		_brick.Die();
		_brick = null;
	}

}