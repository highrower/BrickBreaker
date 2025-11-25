using UnityEngine;

public class Ball : MonoBehaviour {
	private                  Rigidbody2D _rigidbody;
	[SerializeField] private float       speed = 10f;
	[SerializeField] private Vector3     startPosition;
	// TODO: ADD max speed and min speed and all that junk. including damage and whatever

	void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		Launch();
	}

	void Launch() {
		transform.localPosition = startPosition;
		float      randomAngle = Random.Range(-30f, 30f);
		Quaternion rotation    = Quaternion.Euler(0, 0, randomAngle);
		Vector2    direction   = rotation * Vector2.down;
		_rigidbody.linearVelocity = direction * speed;
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Respawn"))
			Launch();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Brick")) {
			var brick = collision.gameObject.GetComponent<Brick>();
			brick.TakeDamage();
		}
	}
}