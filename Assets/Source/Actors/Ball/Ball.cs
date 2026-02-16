using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
	[SerializeField] Vector3      startPosition;
	[SerializeField] BallModel model;
	[SerializeField] BallProgress progress;

	Rigidbody2D _rb;

	public int CurrentDamage
	{
		get
		{
			var tierData = model.GetDamageTier(progress.DamageLevel);

			var efficiency = Mathf.InverseLerp(model.minSpeed,
											   model.maxSpeed,
											   _rb.linearVelocity.magnitude);

			var damage = Mathf.Lerp(tierData.minDamage, tierData.maxDamage, efficiency);

			return Mathf.RoundToInt(damage);
		}
	}

	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		Launch();
	}

	void Update()
	{
		var targetSpeed = Mathf.Clamp(_rb.linearVelocity.magnitude,
									  model.minSpeed,
									  model.maxSpeed);

		if (!Mathf.Approximately(_rb.linearVelocity.magnitude, targetSpeed) &&
			_rb.linearVelocity.magnitude > .01f)
			_rb.linearVelocity = _rb.linearVelocity.normalized * targetSpeed;
	}

	IEnumerator RespawnRoutine()
	{
		_rb.linearVelocity = Vector2.zero; 
		transform.localPosition = startPosition;
        
		// TODO: disable the SpriteRenderer or Collider here
		// to make the ball to be invisible while it waits.
		// HINT, make func in ballView + make ballview

		yield return new WaitForSeconds(model.respawnTime);

		Launch();
	}

	void Launch()
	{
		// TODO: Re-enable SpriteRenderer/Collider here 
        
		var randomAngle = Random.Range(-30f, 30f);
		var rotation    = Quaternion.Euler(0, 0, randomAngle);
		var direction   = rotation * Vector2.down;
		_rb.linearVelocity = direction * model.minSpeed;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Respawn"))
			StartCoroutine(RespawnRoutine());
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent(out Brick brick))
			brick.TakeDamage(CurrentDamage);

		if (collision.gameObject.TryGetComponent(out TwistComponent paddle))
			paddle.RegisterHit();
	}
}