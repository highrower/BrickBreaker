using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Brick : MonoBehaviour {
	public enum BrickType {
		Dirt,
		Cement,
		Steel,
	}

	private readonly Dictionary<BrickType, int> _typeToHealth = new() {
		{ BrickType.Dirt, 1 },
		{ BrickType.Cement, 2 },
		{ BrickType.Steel, 3 },
	};


	private TMP_Text      _healthLabel;
	private BoxCollider2D _collider;
	public  float         GridX { get; set; }
	public  float         GridY { get; set; }

	public float CurrentHealth { get; private set; }

	private void Awake() {
		_collider    = GetComponent<BoxCollider2D>();
		_healthLabel = GetComponentInChildren<TMP_Text>();
	}

	public void Initialize(BrickType type, float x, float y) {
		CurrentHealth = _typeToHealth[type];
		GridX         = x;
		GridY         = y;
		UpdateVisuals();
	}

	public void SetIsTrigger(bool isTrigger) { _collider.isTrigger = isTrigger; }

	private void UpdateVisuals() {
		var spriteRenderer = GetComponent<SpriteRenderer>();
		_healthLabel.text = Mathf.Round(CurrentHealth).ToString(CultureInfo.InvariantCulture);

		if ((int)(CurrentHealth)    == 1) spriteRenderer.color = Color.white;
		else if ((int)CurrentHealth == 2) spriteRenderer.color = Color.green;
		else spriteRenderer.color                              = Color.red;
	}

	public void TakeDamage(float damage = 1) {
		CurrentHealth -= damage;
		if (CurrentHealth <= 0 || MathF.Floor(CurrentHealth) <= 0)
			Destroy(gameObject);
		else {
			UpdateVisuals();
		}
	}
}