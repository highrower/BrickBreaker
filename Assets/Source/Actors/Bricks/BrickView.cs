using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class BrickView : MonoBehaviour {
	SpriteRenderer _renderer;
	TMP_Text       _healthLabel;

	[SerializeField] float textPadding = 0.8f;

	Vector3 _initialScale;
	Vector3 _originalLocalPos;
	bool _isShaking = false;

	void Awake() {
		_renderer     = GetComponent<SpriteRenderer>();
		_healthLabel  = GetComponentInChildren<TMP_Text>();
		_initialScale = _healthLabel.transform.localScale;
		_originalLocalPos = transform.localPosition;
	}

	public void Refresh(int currHealth, BrickSettings settings) {
		ToggleVisible(currHealth > 0);
		_healthLabel.text = currHealth.ToString(CultureInfo.CurrentCulture);

		if (settings.damageSprites == null || settings.damageSprites.Length == 0) return;

		var healthPercent = Mathf.Clamp01((float)currHealth / settings.maxHealth);
		var damagePercent = 1f - healthPercent;

		var spriteIndex = Mathf.FloorToInt(damagePercent * (settings.damageSprites.Length - 1));
		spriteIndex = Mathf.Clamp(spriteIndex, 0, settings.damageSprites.Length - 1);
		if (spriteIndex == 0 && damagePercent > 0f) spriteIndex = 1;

		_renderer.sprite = settings.damageSprites[spriteIndex];
	}
	
	public void Shake(float duration = 0.3f, float magnitude = 0.15f) {
		StopAllCoroutines();
		if(_isShaking) transform.localPosition = _originalLocalPos; 
		StartCoroutine(ShakeRoutine(duration, magnitude));
	}
	
	IEnumerator ShakeRoutine(float duration, float magnitude) {
		_isShaking = true;
        
		_originalLocalPos = transform.localPosition;
		var anchorPos = transform.localPosition;

		var elapsed = 0f;

		while (elapsed < duration) {
			var x = Random.Range(-1f, 1f) * magnitude;
			var y = Random.Range(-1f, 1f) * magnitude;

			transform.localPosition = anchorPos + new Vector3(x, y, 0);

			elapsed += Time.deltaTime;
			magnitude = Mathf.Lerp(magnitude, 0f, elapsed / duration);
			yield return null;
		}

		transform.localPosition = anchorPos;
		_isShaking = false;
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