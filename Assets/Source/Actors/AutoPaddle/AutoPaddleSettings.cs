using UnityEngine;

public class AutoPaddleSettings : ScriptableObject, ISaveable
{
    public string ID { get; }
    public bool Unlocked;
    public int SizeLevel { get; }
    public int SpeedLevel;
    
    public void Save(SaveData data)
    {
        throw new System.NotImplementedException();
    }

    public void Load(SaveData data)
    {
        throw new System.NotImplementedException();
    }
}