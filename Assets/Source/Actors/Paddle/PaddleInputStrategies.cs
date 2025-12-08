using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.InputSystem;

public interface IPaddleInput {
	float GetTargetX(Camera cam, Rect bounds, float halfWidth);

	bool IsTwisting(Camera cam, out float twistAngle);
}

public class MobilePaddleInput : IPaddleInput {
	public float GetTargetX(Camera cam, Rect bounds, float halfWidth) {
		if (Touch.activeTouches.Count == 0) return bounds.center.x;

		var screenPos = Touch.activeTouches[0].screenPosition;
		var worldPos  = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
		return Mathf.Clamp(worldPos.x, bounds.xMin + halfWidth, bounds.xMax - halfWidth);
	}

	public bool IsTwisting(Camera cam, out float twistAngle) {
		twistAngle = 90;
		if (Touch.activeTouches.Count < 2) return false;

		var t2    = Touch.activeTouches[1];
		var curr  = cam.ScreenToWorldPoint((Vector3)t2.screenPosition      + Vector3.forward * 10);
		var start = cam.ScreenToWorldPoint((Vector3)t2.startScreenPosition + Vector3.forward * 10);

		twistAngle = 90 + (start.y - curr.y) * 45;
		return true;
	}
}
public class EditorPaddleInput : IPaddleInput {
	public float GetTargetX(Camera cam, Rect bounds, float halfWidth) {
		if (Mouse.current == null) return bounds.center.x;

		var mousePos = Mouse.current.position.ReadValue();
		var worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

		return Mathf.Clamp(worldPos.x, bounds.xMin + halfWidth, bounds.xMax - halfWidth);
	}

	public bool IsTwisting(Camera cam, out float twistAngle) {
		twistAngle = 90;
		if (Keyboard.current == null) return false;

		var upPressed   = Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed;
		var downPressed = Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;

		if (!upPressed && !downPressed) return false;

		var simulatedYDiff              = 0f;
		if (upPressed) simulatedYDiff   = -2f;
		if (downPressed) simulatedYDiff = 2f;

		twistAngle = 90 + (simulatedYDiff * 45);
		return true;
	}
}