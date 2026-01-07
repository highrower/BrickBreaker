using UnityEngine;


[CreateAssetMenu(menuName = "Settings/Ball Settings")]
public class BallSettings : ScriptableObject, ISaveable {
	[SerializeField] string id = "Ball_Damage";

	public string       ID => id;
	public DamageTier[] damageTiers;
	public float        minSpeed              = 5f;
	public float        maxSpeed              = 15f;
	public float        speedThreshold        = 5f;
	public float        lookAheadDistance     = .5f;
	public float        bustThroughResistance = 0.8f;

	[System.NonSerialized] public int DamageLevel;

	public DamageTier CurrentDamageTier => GetTierData(DamageLevel);

	public void Save(SaveData data) => data.UpgradeIdToLevel[ID] = DamageLevel;

	public void Load(SaveData data) => data.UpgradeIdToLevel.TryGetValue(ID, out DamageLevel);

	void OnEnable() => DamageLevel = 0;

	DamageTier GetTierData(int tier) => damageTiers[Mathf.Clamp(tier, 0, damageTiers.Length - 1)];


}

[System.Serializable]
public struct DamageTier {
	public int minDamage;
	public int maxDamage;
}