using System.Collections;
using UnityEngine;

public class TwistComponent : MonoBehaviour {
	[Header("Settings")]
	[SerializeField] float snapDuration = 0.5f;

	[SerializeField] AnimationCurve snapCurve;

	Rigidbody2D _rb;
	Coroutine   _currentTween;
	Paddle      _paddle;


	void Awake() {
		_rb     = GetComponent<Rigidbody2D>();
		_paddle = GetComponent<Paddle>();
	}

	void OnEnable() {
		if (_paddle != null) _paddle.DragRelease += ReleaseTwist;
	}

	void OnDisable() {
		if (_paddle != null) _paddle.DragRelease -= ReleaseTwist;
	}

	void ReleaseTwist() {
		if (_currentTween != null) StopCoroutine(_currentTween);
		_currentTween = StartCoroutine(SnapRoutine());
	}

	IEnumerator SnapRoutine() {
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
	}
}