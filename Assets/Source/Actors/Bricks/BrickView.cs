using System.Globalization;
using TMPro;
using UnityEngine;

public class BrickView : MonoBehaviour {
	SpriteRenderer _renderer;
	TMP_Text       _healthLabel;

	[SerializeField] float textPadding = 0.8f;

	Vector3 _initialScale;

	void Awake() {
		_renderer     = GetComponent<SpriteRenderer>();
		_healthLabel  = GetComponentInChildren<TMP_Text>();
		_initialScale = _healthLabel.transform.localScale;
	}

	public void Refresh(int currHealth, BrickSettings settings) {
		ToggleVisible(currHealth > 0);
		_healthLabel.text = currHealth.ToString(CultureInfo.CurrentCulture);

		if (settings.damageSprites == null || settings.damageSprites.Length == 0) return;
		var healthPercent = Mathf.Clamp01(currHealth              / settings.maxHealth);
		var spriteIndex   = Mathf.FloorToInt((1f - healthPercent) * settings.damageSprites.Length);
		spriteIndex      = Mathf.Clamp(spriteIndex, 0, settings.damageSprites.Length - 1);
		_renderer.sprite = settings.damageSprites[spriteIndex];
	}

	void ToggleVisible(bool isVisible) {
		_renderer.enabled    = isVisible;
		_healthLabel.enabled = isVisible;
	}

	public void SetScale(Vector2 newScale) {
		if (_renderer)
			_renderer.size = newScale;
		if (_healthLabel)
			CorrectTextScale(newScale);
	}

	void CorrectTextScale(Vector2 brickSize) {
		var minDimension = Mathf.Min(brickSize.x, brickSize.y);
		var scaleFactor  = minDimension / 2f;
		_healthLabel.transform.localScale = _initialScale * scaleFactor;

		var visualCenter = _renderer.localBounds.center;
		visualCenter.z                       = -0.1f;
		_healthLabel.transform.localPosition = visualCenter;
	}
}