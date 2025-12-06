using UnityEngine;

public class Drop : MonoBehaviour {
	[SerializeField] private float        dropSpeed = 5.0f;
	[SerializeField] private DropSettings settings;

	public int Value => settings.value;

	private void FixedUpdate() =>
		transform.position += Vector3.down * (dropSpeed * Time.fixedDeltaTime);

}

[CreateAssetMenu(menuName = "Settings/DropSettings")]
public class DropSettings : ScriptableObject {
	public int value;
}