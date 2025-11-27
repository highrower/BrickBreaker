using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {
	[SerializeField] private float       minSpeed = 5f;
	[SerializeField] private float       maxSpeed = 15f;
	[SerializeField] private Vector3     startPosition;
	[SerializeField] private float       speedToDamageRatio = 0.2f;
	private                  Rigidbody2D _rb;

	public float   CurrentDamage   => _rb.linearVelocity.magnitude * speedToDamageRatio;
	public Vector2 CurrentVelocity => _rb.linearVelocity;

	void Start() {
		_rb = GetComponent<Rigidbody2D>();
		Launch();
	}

	void Update() {
		// TODO: make it so the weird interaction with the ball and
		// the paddle at the edge of the screen doesnt make the ball get stuck on the edge of the wall
		var targetSpeed = Mathf.Clamp(_rb.linearVelocity.magnitude, minSpeed, maxSpeed);
		if (!Mathf.Approximately(_rb.linearVelocity.magnitude, targetSpeed) && _rb.linearVelocity.magnitude > .01f)
			_rb.linearVelocity = _rb.linearVelocity.normalized * targetSpeed;
	}


	void Launch() {
		transform.localPosition = startPosition;
		float      randomAngle = Random.Range(-30f, 30f);
		Quaternion rotation    = Quaternion.Euler(0, 0, randomAngle);
		Vector2    direction   = rotation * Vector2.down;
		_rb.linearVelocity = direction * minSpeed;
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Respawn"))
			Launch();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Brick")) {
			var brick = collision.gameObject.GetComponent<Brick>();
			brick.TakeDamage(CurrentDamage);
		}
	}
}