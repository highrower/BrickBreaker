// This is where data that is affected by upgrades lives. makes sense to have it be a normal class
// because this shouldnt be modifyable in a serialized way since
// it will only read from various upgrades/level shopupgrades
public class BallProgress: ISaveable
{
    public string ID =>  "Ball Progress";
    public int DamageLevel;
    
    public void Save(SaveData data) => data.UpgradeIdToLevel[ID] = DamageLevel;

    public void Load(SaveData data) => data.UpgradeIdToLevel.TryGetValue(ID, out DamageLevel);
    
}