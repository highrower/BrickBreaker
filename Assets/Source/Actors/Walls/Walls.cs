using System;
using NUnit.Framework;
using UnityEngine;

public class Walls : MonoBehaviour {
	private GameObject _leftWall;
	private GameObject _rightWall;
	private GameObject _topWall;

	[SerializeField] private RectReference playAreaBounds;

	private void Awake() {
		if (transform.Find("LeftWall")) _leftWall   = transform.Find("LeftWall").gameObject;
		if (transform.Find("RightWall")) _rightWall = transform.Find("RightWall").gameObject;
		if (transform.Find("UpperWall")) _topWall   = transform.Find("UpperWall").gameObject;
		Assert.True(_leftWall != null && _rightWall != null && _topWall != null,
		            "Walls: One or more wall GameObjects not found as children.");
	}

	private void Start() {
		playAreaBounds.variable.OnValueChanged += AlignWalls;
		AlignWalls(playAreaBounds);
	}

	private void OnDestroy() { playAreaBounds.variable.OnValueChanged -= AlignWalls; }

	private void AlignWalls(Rect bounds) {
		MoveWall(_topWall,   new Vector2(bounds.center.x, bounds.yMax),     Vector2.up);
		MoveWall(_leftWall,  new Vector2(bounds.xMin,     bounds.center.y), Vector2.left);
		MoveWall(_rightWall, new Vector2(bounds.xMax,     bounds.center.y), Vector2.right);
	}

	private static void MoveWall(GameObject wall, Vector2 targetEdge, Vector2 directionOut) {
		var col     = wall.GetComponent<BoxCollider2D>();
		var extents = col.bounds.extents;
		var offset  = directionOut * (directionOut.x != 0 ? extents.x : extents.y);

		wall.transform.position = new Vector3(targetEdge.x + offset.x,
		                                      targetEdge.y + offset.y,
		                                      wall.transform.position.z);
	}
}