using UnityEngine;


[CreateAssetMenu(menuName = "Settings/BallSettings")]
public class BallSettings : ScriptableObject {
	public float minDamage             = 1f;
	public float maxDamage             = 5f;
	public float minSpeed              = 5f;
	public float maxSpeed              = 15f;
	public float speedThreshold        = 10f;
	public float lookAheadDistance     = .5f;
	public float bustThroughResistance = 0.8f;
}