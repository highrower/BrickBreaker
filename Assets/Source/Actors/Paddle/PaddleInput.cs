using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public static class PaddleInput {

	public static float GetTargetX(Camera cam, Rect bounds, float halfWidth) {
		var screenPos = GetTouchPos() ?? GetMousePos();
		if (screenPos == null) return bounds.center.x;

		var worldX = cam.ScreenToWorldPoint((Vector3)screenPos + Vector3.forward * 10).x;
		return Mathf.Clamp(worldX, bounds.xMin + halfWidth, bounds.xMax - halfWidth);
	}

	public static bool IsTwisting(Camera cam, out float twistAngle) {
		var twistVal = GetTouchTwist(cam) ?? GetKeyboardTwist() ?? 0f;
		twistAngle = 90 + twistVal * 45;
		return Mathf.Abs(twistVal) > 0.01f;
	}

	static Vector2? GetTouchPos() => Touch.activeTouches.Count > 0 ? Touch.activeTouches[0].screenPosition : null;

	static Vector2? GetMousePos() => Mouse.current != null && Mouse.current.leftButton.isPressed ?
		Mouse.current.position.ReadValue() :
		null;

	static float? GetTouchTwist(Camera cam) {
		if (Touch.activeTouches.Count < 2) return null;
		var t2    = Touch.activeTouches[1];
		var curr  = cam.ScreenToWorldPoint((Vector3)t2.screenPosition      + Vector3.forward * 10);
		var start = cam.ScreenToWorldPoint((Vector3)t2.startScreenPosition + Vector3.forward * 10);
		var yDiff = curr.y - start.y;
		return (Touch.activeTouches[0].screenPosition.x > t2.screenPosition.x) ? -yDiff : yDiff;
	}

	static float? GetKeyboardTwist() {
		if (Keyboard.current == null) return null;
		float? val                                                                          = null;
		if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) val   = -2f;
		if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) val = 2f;
		return val;
	}
}