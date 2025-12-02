using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Brick : MonoBehaviour {
	[SerializeField] private float         health;
	[SerializeField] private TMP_Text      healthLabel;
	private                  BoxCollider2D _collider;
	public                   float         GridX { get; set; }
	public                   float         GridY { get; set; }

	public float CurrentHealth => health;

	void Awake() { _collider = GetComponent<BoxCollider2D>(); }

	public void Initialize(float startHealth, float x, float y) {
		health = startHealth;
		GridX  = x;
		GridY  = y;
		UpdateVisuals();
	}

	public void SetIsTrigger(bool isTrigger) { _collider.isTrigger = isTrigger; }

	void UpdateVisuals() {
		var spriteRenderer = GetComponent<SpriteRenderer>();
		healthLabel.text = Mathf.Round(health).ToString(CultureInfo.InvariantCulture);

		if ((int)(health)    == 1) spriteRenderer.color = Color.white;
		else if ((int)health == 2) spriteRenderer.color = Color.green;
		else spriteRenderer.color                       = Color.red;
	}

	public void TakeDamage(float damage = 1) {
		health -= damage;
		if (health <= 0 || MathF.Floor(health) <= 0)
			Destroy(gameObject);
		else {
			UpdateVisuals();
		}
	}
}