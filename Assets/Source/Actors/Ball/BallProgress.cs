// This is where data that is affected by upgrades lives. makes sense to have it be a normal class
// because this shouldnt be modifyable in a serialized way since
// it will only read from various upgrades/level shopupgrades
using UnityEngine;

[CreateAssetMenu(menuName = "Progress/Ball Progress")]
public class BallProgress: ScriptableObject, ISaveable
{
    public string ID =>  "ball";
    
    public int damageLevel;
    
    public void Save(SaveData data) => data.UpgradeIdToLevel[ID] = damageLevel;

    public void Load(SaveData data)
    {
        if(data.UpgradeIdToLevel.TryGetValue(ID, out var d)) damageLevel = d;  
    } 
    
}