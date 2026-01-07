using UnityEngine;

public class PaddleView : MonoBehaviour {
	SpriteRenderer _spriteRenderer;

	public void Start() { _spriteRenderer = GetComponent<SpriteRenderer>(); }

	public void SetTwistView(bool isTwist) {
		if (!_spriteRenderer) return;

		var tempColor = _spriteRenderer.color;
		tempColor.a           = isTwist ? 0.5f : 1f;
		_spriteRenderer.color = tempColor;
	}
}