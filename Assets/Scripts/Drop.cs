using UnityEngine;

public class Drop : MonoBehaviour {
	[SerializeField] private float dropSpeed = 5.0f;

	private void FixedUpdate() {
		transform.position += Vector3.down * (dropSpeed * Time.fixedDeltaTime);
	}
}