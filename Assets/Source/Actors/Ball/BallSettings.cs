using UnityEngine;


[CreateAssetMenu(menuName = "Settings/Ball Settings")]
public class BallSettings : ScriptableObject {
	[System.Serializable]
	public struct DamageTier {
		public int minDamage;
		public int maxDamage;
	}

	[Header("Balancing")]
	public DamageTier[] damageTiers;

	[Header("Movement")]
	public float minSpeed = 5f;

	public float maxSpeed              = 15f;
	public float speedThreshold        = 10f;
	public float lookAheadDistance     = .5f;
	public float bustThroughResistance = 0.8f;

	public DamageTier GetTierData(int tier) {
		return damageTiers[Mathf.Clamp(tier, 0, damageTiers.Length - 1)];
	}
}