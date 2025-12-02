using UnityEngine;

public class BallBustThrough : MonoBehaviour {
	[SerializeField] private float     speedThreshold        = 10f;
	[SerializeField] private float     lookAheadDistance     = .5f;
	[SerializeField] private float     bustThroughResistance = 0.8f;
	[SerializeField] private LayerMask brickLayer;

	private Rigidbody2D      _rb;
	private CircleCollider2D _collider;
	private Brick            _brick = null;
	private Ball             _ball;


	private void Awake() {
		_rb       = GetComponent<Rigidbody2D>();
		_collider = GetComponent<CircleCollider2D>();
		_ball     = GetComponent<Ball>();
	}

	private void FixedUpdate() {
		if (_brick != null)
			return;
		CheckForFutureBricks();
	}

	private void CheckForFutureBricks() {
		if (_rb.linearVelocity.magnitude < speedThreshold) return;

		var hit = Physics2D.CircleCast(transform.position,
		                               _collider.radius,
		                               _rb.linearVelocity.normalized,
		                               lookAheadDistance,
		                               brickLayer);

		if (hit.collider != null) {
			if (hit.collider.TryGetComponent<Brick>(out var brick)) {
				if (brick.CurrentHealth < _ball.CurrentDamage) {
					_brick = brick;
					brick.SetIsTrigger(true);
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<Brick>(out var brick) && brick == _brick)
			BreakThrough();
	}

	private void BreakThrough() {
		_rb.linearVelocity *= bustThroughResistance / _brick.CurrentHealth;
		Destroy(_brick.gameObject);
		_brick = null;
	}

}