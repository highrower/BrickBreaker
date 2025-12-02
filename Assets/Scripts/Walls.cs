using System;
using UnityEngine;

public class Walls : MonoBehaviour {
	[SerializeField] private GameObject leftWall;
	[SerializeField] private GameObject rightWall;
	[SerializeField] private GameObject topWall;

	private void Awake() {
		var cam   = Camera.main;
		var zDist = Mathf.Abs(leftWall.transform.position.z - cam.transform.position.z);

		var leftEdge = cam.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, zDist));
		leftWall.transform.position = leftEdge;

		var rightEdge = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, zDist));
		rightWall.transform.position = rightEdge;

		var topEdge = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height, zDist));
		topWall.transform.position = topEdge;
	}
}