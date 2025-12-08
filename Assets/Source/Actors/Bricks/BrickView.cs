using System.Globalization;
using TMPro;
using UnityEngine;

public class BrickView : MonoBehaviour {
	SpriteRenderer _renderer;
	TMP_Text       _healthLabel;

	void Awake() {
		_renderer    = GetComponent<SpriteRenderer>();
		_healthLabel = GetComponentInChildren<TMP_Text>();
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
}