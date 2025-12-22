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

	public void CorrectTextScale(Vector3 parentScale) {
		if (!_healthLabel) return;

		var minDimension    = Mathf.Min(parentScale.x, parentScale.y);
		var targetWorldSize = minDimension                      * textPadding;
		var newX            = (targetWorldSize / parentScale.x) * _initialScale.x;
		var newY            = (targetWorldSize / parentScale.y) * _initialScale.y;
		_healthLabel.transform.localScale = new Vector3(newX, newY, 1f);

		var visualCenter = _renderer.localBounds.center;
		visualCenter.z                       = -0.1f;
		_healthLabel.transform.localPosition = visualCenter;
	}
}