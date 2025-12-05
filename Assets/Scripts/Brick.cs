using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class Brick : MonoBehaviour {
	private TMP_Text       _healthLabel;
	private BoxCollider2D  _collider;
	private SpriteRenderer _renderer;
	private BrickSettings  _settings;
	private bool           _isDead;

	[SerializeField] private float     spawnRatePerSecond = 0.5f;
	[SerializeField] private LayerMask ballLayer;

	public float GridX         { get; set; }
	public float GridY         { get; set; }
	public float CurrentHealth { get; private set; }

	private void Awake() {
		_collider    = GetComponent<BoxCollider2D>();
		_renderer    = GetComponent<SpriteRenderer>();
		_healthLabel = GetComponentInChildren<TMP_Text>();
	}

	public void Initialize(BrickSettings settings, float x, float y) {
		_settings     = settings;
		CurrentHealth = _settings.hp;
		GridX         = x;
		GridY         = y;
		UpdateVisuals();
	}

	public void SetIsTrigger(bool isTrigger) { _collider.isTrigger = isTrigger; }

	public void TakeDamage(float damage = 1) {
		CurrentHealth -= damage;
		if (CurrentHealth <= 0 || MathF.Floor(CurrentHealth) <= 0)
			Die();
		UpdateVisuals();
	}

	public void Die() {
		if (_isDead) return;

		_isDead              = true;
		_collider.enabled    = false;
		_renderer.enabled    = false;
		_healthLabel.enabled = false;

		if (_settings.drop != null && Random.value <= _settings.dropChance)
			Instantiate(_settings.drop, transform.position, Quaternion.identity);
		StartCoroutine(SpawnRoutine());
	}

	private IEnumerator SpawnRoutine() {
		while (true) {
			yield return new WaitForSeconds(1f / spawnRatePerSecond);
			if (UnityEngine.Random.value <= _settings.spawnChance)
				break;
		}

		while (IsBallInside()) {
			yield return new WaitForSeconds(0.1f);
		}

		Respawn();
	}

	private bool IsBallInside() {
		if (!_collider) return false;

		var boxSize = _collider.size;
		boxSize.x *= transform.localScale.x;
		boxSize.y *= transform.localScale.y;

		return Physics2D.OverlapBox(transform.position,
		                            boxSize * 0.9f,
		                            0f,
		                            ballLayer);
	}

	private void Respawn() {
		CurrentHealth        = _settings.hp;
		_isDead              = false;
		_collider.enabled    = true;
		_renderer.enabled    = true;
		_healthLabel.enabled = true;
		UpdateVisuals();
	}

	private void UpdateVisuals() {
		_healthLabel.text = Mathf.Round(CurrentHealth).ToString(CultureInfo.InvariantCulture);

		if ((int)(CurrentHealth)    == 1) _renderer.color = Color.white;
		else if ((int)CurrentHealth == 2) _renderer.color = Color.green;
		else _renderer.color                              = Color.red;
	}
}