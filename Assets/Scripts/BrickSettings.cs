using UnityEngine;


[CreateAssetMenu(menuName = "Settings/BrickSettings")]
public class BrickSettings : ScriptableObject {
	[SerializeField] public float hp;
	[SerializeField] public float spawnChance;
}