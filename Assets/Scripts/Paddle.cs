using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Paddle : MonoBehaviour {
	[SerializeField] private GameObject leftWall;
	[SerializeField] private GameObject rightWall;

	private Camera _cam;
	private float  _maxBoundary;
	private float  _minBoundary;

	private void Start()
	{
		_cam = Camera.main;
		RecalculateBoundaries();
	}

	private void Update()
	{
		if (Touch.activeTouches.Count > 0)
		{
			var screenPosition = Touch.activeTouches[0].screenPosition;
			var worldPosition  = _cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
			var clampedX       = Mathf.Clamp(worldPosition.x, _minBoundary, _maxBoundary);

			transform.position = Vector3.Lerp(
				transform.position, new Vector3(clampedX, transform.position.y, transform.position.z),
				Time.deltaTime * 10
			);
		}
	}

	private void OnEnable() { EnhancedTouchSupport.Enable(); }

	private void OnDisable() { EnhancedTouchSupport.Disable(); }

	private void RecalculateBoundaries()
	{
		var paddleHalfWidth    = GetComponent<SpriteRenderer>().bounds.extents.x;
		var leftWallInnerEdge  = leftWall.GetComponent<SpriteRenderer>().bounds.max.x;
		var rightWallInnerEdge = rightWall.GetComponent<SpriteRenderer>().bounds.min.x;

		_minBoundary = leftWallInnerEdge  + paddleHalfWidth;
		_maxBoundary = rightWallInnerEdge - paddleHalfWidth;
	}
}