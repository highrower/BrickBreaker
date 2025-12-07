using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Brick Settings")]
public class BrickSettings : ScriptableObject {
	public int        maxHealth;
	public float      spawnChance;
	public GameObject drop;
	public float      dropChance;

	[Header("Visuals")]
	public Sprite[] damageSprites;
}