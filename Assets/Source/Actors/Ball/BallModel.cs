using UnityEngine;

// This is where immutable ball data lives. so things not affected by any sort of upgrades
[CreateAssetMenu(menuName = "Settings/Ball Settings")]
public class BallModel : ScriptableObject
{
	public float        minSpeed              = 5f;
	public float        maxSpeed              = 15f;
	public float        speedThreshold        = 5f;
	public float        lookAheadDistance     = .5f;
	public float        bustThroughResistance = 0.8f;
	public float        respawnTime           = 5f;
	
	public DamageTier[] damageTiers;
	
	public DamageTier GetDamageTier(int level)
	{
		if (damageTiers == null || damageTiers.Length == 0)
			return default;
		return damageTiers[Mathf.Clamp(level, 0, damageTiers.Length - 1)];
	}}

[System.Serializable]
public struct DamageTier
{
	public int minDamage;
	public int maxDamage;
}
