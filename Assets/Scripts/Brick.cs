using UnityEngine;

public class Brick : MonoBehaviour {
	public int health;

	public void Initialize(int startHealth) {
		health = startHealth;
		UpdateVisuals();
	}

	void UpdateVisuals() {
		var spriteRenderer = GetComponent<SpriteRenderer>();

		if (health      == 1) spriteRenderer.color = Color.white;
		else if (health == 2) spriteRenderer.color = Color.green;
		else spriteRenderer.color                  = Color.red;
	}

	public void TakeDamage() {
		health--;
		if (health <= 0) {
			Destroy(gameObject);
		}
		else {
			UpdateVisuals();
		}
	}
}