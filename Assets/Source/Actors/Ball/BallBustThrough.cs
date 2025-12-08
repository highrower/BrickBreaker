using UnityEngine;

public class BallBustThrough : MonoBehaviour {
	[SerializeField] LayerMask    brickLayer;
	[SerializeField] BallSettings settings;

	Rigidbody2D      _rb;
	CircleCollider2D _collider;
	Brick            _brick;
	Ball             _ball;


	void Awake() {
		_rb       = GetComponent<Rigidbody2D>();
		_collider = GetComponent<CircleCollider2D>();
		_ball     = GetComponent<Ball>();
	}

	void FixedUpdate() {
		CheckForFutureBricks();
	}

	void CheckForFutureBricks() {
		if (_brick) {
			var distance = Vector2.Distance(transform.position, _brick.transform.position);

			if (!(distance > settings.lookAheadDistance * 2.0f))
				return;

			_brick.SetIsTrigger(false);
			_brick = null;

			return;
		}

		if (_rb.linearVelocity.magnitude < settings.speedThreshold) return;

		var hit = Physics2D.CircleCast(transform.position,
		                               _collider.radius * transform.lossyScale.x,
		                               _rb.linearVelocity.normalized,
		                               settings.lookAheadDistance,
		                               brickLayer);

		if (!hit.collider)
			return;
		if (hit.collider.TryGetComponent<Brick>(out var brick))
			if (brick.CurrentHealth > _ball.CurrentDamage)
				return;
		_brick = brick;
		brick.SetIsTrigger(true);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent<Brick>(out var brick) && brick == _brick)
			BreakThrough();
	}

	void BreakThrough() {
		Debug.Log($"Breaking through: {_brick.name}"); // Proof it wasn't null
		_rb.linearVelocity *= settings.bustThroughResistance / _brick.CurrentHealth;
		_brick.Die();
		_brick = null;
	}

	// Add this to BallBustThrough.cs
	void OnDrawGizmos() {
		if (_rb == null || _collider == null) return;

		Gizmos.color = Color.red;
		var direction = _rb.linearVelocity.normalized;
		var distance  = settings.lookAheadDistance;

		Gizmos.DrawRay(transform.position, direction                                                 * distance);
		Gizmos.DrawWireSphere(transform.position + (Vector3)(direction * distance), _collider.radius * transform.lossyScale.x);
	}
}