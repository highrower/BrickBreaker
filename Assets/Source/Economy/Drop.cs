using System;
using UnityEngine;
using UnityEngine.Pool;

public class Drop : MonoBehaviour {
	[SerializeField] private float             dropSpeed = 5.0f;
	[SerializeField] private DropSettings      settings;
	private                  IObjectPool<Drop> _pool;

	public int Value => settings.value;

	private void FixedUpdate() =>
		transform.position += Vector3.down * (dropSpeed * Time.fixedDeltaTime);

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Respawn"))
			Deactivate();
	}

	public void SetPool(IObjectPool<Drop> pool) {
		_pool = pool;
	}

	public void Deactivate() {
		if (_pool != null)
			_pool.Release(this);
		else
			Destroy(gameObject);
	}

}

[CreateAssetMenu(menuName = "Settings/DropSettings")]
public class DropSettings : ScriptableObject {
	public int value;
}