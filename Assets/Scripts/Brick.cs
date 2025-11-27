using UnityEngine;

public class Brick : MonoBehaviour {
	public  float         health;
	private BoxCollider2D _collider;

	void Awake() { _collider = GetComponent<BoxCollider2D>(); }

	public void Initialize(float startHealth) {
		health = startHealth;
		UpdateVisuals();
	}

	public void SetIsTrigger(bool isTrigger) { _collider.isTrigger = isTrigger; }

	void UpdateVisuals() {
		var spriteRenderer = GetComponent<SpriteRenderer>();

		if (Mathf.Approximately(health,      1)) spriteRenderer.color = Color.white;
		else if (Mathf.Approximately(health, 2)) spriteRenderer.color = Color.green;
		else spriteRenderer.color                                     = Color.red;
	}

	public void TakeDamage(float damage = 1) {
		health -= damage;
		if (health <= 0) {
			Destroy(gameObject);
		}
		else {
			UpdateVisuals();
		}
	}
}