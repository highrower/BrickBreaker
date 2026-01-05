using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {
	[SerializeField] Vector3      startPosition;
	[SerializeField] BallSettings settings;

	Rigidbody2D _rb;

	public int CurrentDamage {
		get {
			var tierData   = settings.CurrentDamageTier;
			var efficiency = Mathf.InverseLerp(settings.minSpeed, settings.maxSpeed, _rb.linearVelocity.magnitude);
			var damage     = Mathf.Lerp(tierData.minDamage, tierData.maxDamage, efficiency);
			return Mathf.RoundToInt(damage);
		}
	}

	void Start() {
		_rb = GetComponent<Rigidbody2D>();
		Launch();
	}

	void Update() {
		// TODO: make it so the weird interaction with the ball and
		// the paddle at the edge of the screen doesnt make the ball get stuck on the edge of the wall
		var targetSpeed = Mathf.Clamp(_rb.linearVelocity.magnitude, settings.minSpeed, settings.maxSpeed);
		if (!Mathf.Approximately(_rb.linearVelocity.magnitude, targetSpeed) && _rb.linearVelocity.magnitude > .01f)
			_rb.linearVelocity = _rb.linearVelocity.normalized * targetSpeed;
	}


	void Launch() {
		transform.localPosition = startPosition;
		var randomAngle = Random.Range(-30f, 30f);
		var rotation    = Quaternion.Euler(0, 0, randomAngle);
		var direction   = rotation * Vector2.down;
		_rb.linearVelocity = direction * settings.minSpeed;
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Respawn"))
			Launch();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.gameObject.CompareTag("Brick"))
			return;
		collision.gameObject.GetComponent<Brick>().TakeDamage(CurrentDamage);
	}
}