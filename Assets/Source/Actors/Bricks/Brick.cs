using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class Brick : MonoBehaviour {
	BoxCollider2D _collider;
	BrickSettings _settings;
	BrickView     _view;

	[SerializeField] float     spawnRatePerSecond = 0.5f;
	[SerializeField] LayerMask ballLayer;

	public float GridX         { get; set; }
	public float GridY         { get; set; }
	public int   CurrentHealth { get; private set; }

	void Awake() {
		_collider = GetComponent<BoxCollider2D>();
		_view     = GetComponent<BrickView>();
	}

	public void Initialize(BrickSettings settings, float x, float y) {
		_settings     = settings;
		CurrentHealth = _settings.maxHealth;
		GridX         = x;
		GridY         = y;
		_view.Refresh(CurrentHealth, _settings);
	}

	public void SetIsTrigger(bool isTrigger) { _collider.isTrigger = isTrigger; }

	public void TakeDamage(int damage = 1)
	{
		_view.Shake();
		CurrentHealth -= damage;
		if (CurrentHealth <= 0 || MathF.Floor(CurrentHealth) <= 0)
			Die();
		_view.Refresh(CurrentHealth, _settings);
	}

	public void Die() {
		CurrentHealth     = 0;
		_collider.enabled = false;
		_view.Refresh(CurrentHealth, _settings);

		if (_settings.drop != null && Random.value <= _settings.dropChance)
			DropSpawner.Instance.SpawnDrop(transform.position);
		StartCoroutine(SpawnRoutine());
	}

	IEnumerator SpawnRoutine() {
		while (true) {
			yield return new WaitForSeconds(1f / spawnRatePerSecond);
			if (Random.value <= _settings.spawnChance)
				break;
		}

		while (IsBallInside())
			yield return new WaitForSeconds(0.1f);

		Respawn();
	}

	bool IsBallInside() {
		if (!_collider) return false;

		var boxSize    = _collider.size;
		var localScale = transform.localScale;
		boxSize.x *= localScale.x;
		boxSize.y *= localScale.y;

		return Physics2D.OverlapBox(transform.position,
		                            boxSize * 0.9f,
		                            0f,
		                            ballLayer);
	}

	void Respawn() {
		CurrentHealth       = _settings.maxHealth;
		_collider.enabled   = true;
		_collider.isTrigger = false;
		_view.Refresh(CurrentHealth, _settings);
	}

	public void SetScale(Vector2 newScale) {
		transform.localScale = Vector3.one;

		if (_view) _view.SetScale(newScale);
		if (!_collider)
			return;
		_collider.size   = newScale;
		_collider.offset = Vector2.zero;
	}
}