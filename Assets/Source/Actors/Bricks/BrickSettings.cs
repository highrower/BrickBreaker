using UnityEngine;

[CreateAssetMenu(menuName = "Settings/BrickSettings")]
public class BrickSettings : ScriptableObject {
	public int        maxHealth;
	public float      spawnChance;
	public GameObject drop;
	public float      dropChance;

	[Header("Visuals")]
	public Sprite[] damageSprites;
}