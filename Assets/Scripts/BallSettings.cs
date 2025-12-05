using UnityEngine;


[CreateAssetMenu(menuName = "Settings/BallSettings")]
public class BallSettings : ScriptableObject {
	public float minSpeed              = 5f;
	public float maxSpeed              = 15f;
	public float speedThreshold        = 10f;
	public float lookAheadDistance     = .5f;
	public float bustThroughResistance = 0.8f;
	public float speedToDamageRatio    = 0.2f;
}