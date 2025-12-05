using UnityEngine;


[CreateAssetMenu(menuName = "Settings/BrickSettings")]
public class BrickSettings : ScriptableObject {
	public float      hp;
	public float      spawnChance;
	public GameObject drop;
	public float      dropChance;
}