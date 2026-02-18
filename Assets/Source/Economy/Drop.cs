using System;
using UnityEngine;
using UnityEngine.Pool;

public class Drop : MonoBehaviour
{
	[SerializeField] float        dropSpeed = 5.0f;
	[SerializeField] DropSettings settings;
	IObjectPool<Drop>             _pool;

	public int Value => settings.value;

	void FixedUpdate() =>
		transform.position += Vector3.down * (dropSpeed * Time.fixedDeltaTime);

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Respawn"))
			Deactivate();
	}

	public void SetPool(IObjectPool<Drop> pool)
	{
		_pool = pool;
	}

	public void Deactivate()
	{
		if (_pool != null)
			_pool.Release(this);
		else
			Destroy(gameObject);
	}
}