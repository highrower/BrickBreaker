using System.Collections;
using UnityEngine;

public class TwistComponent : MonoBehaviour {
	[Header("Settings")]
	[SerializeField] float snapDuration = 0.5f;

	[SerializeField] AnimationCurve snapCurve;

	private Rigidbody2D _rb;
	private Coroutine   _currentTween;

	void Awake() => _rb = GetComponent<Rigidbody2D>();

	private void OnEnable() { Paddle.DragRelease += ReleaseTwist; }

	private void OnDisable() { Paddle.DragRelease -= ReleaseTwist; }

	private void ReleaseTwist() {
		if (_currentTween != null) StopCoroutine(_currentTween);
		_currentTween = StartCoroutine(SnapRoutine());
	}

	private IEnumerator SnapRoutine() {
		var         startRot  = _rb.rotation;
		const float targetRot = 90f;
		var         elapsed   = 0f;

		while (elapsed < snapDuration) {
			elapsed += Time.fixedDeltaTime;
			var percent = elapsed / snapDuration;

			var curveValue = snapCurve.Evaluate(percent);
			var newAngle   = Mathf.LerpUnclamped(startRot, targetRot, curveValue);

			_rb.MoveRotation(newAngle);

			yield return new WaitForFixedUpdate();
		}

		_rb.MoveRotation(targetRot);
		_rb.angularVelocity = 0f;
		_rb.angularVelocity = 0f;
	}
}