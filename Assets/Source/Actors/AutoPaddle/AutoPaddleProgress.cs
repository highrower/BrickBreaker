using UnityEngine;

[CreateAssetMenu(menuName = "Progress/Auto Paddle Progress")]

public class AutoPaddleProgress : ScriptableObject, ISaveable
{
    public string ID => "auto_paddle";
    public bool Unlocked;
    public int SizeLevel { get; }
    public int SpeedLevel;
    
    public void Save(SaveData data)
    {
        // implement when your SaveData shape is ready
        // data.SetInt($"{ID}.unlocked", Unlocked ? 1 : 0);
        // data.SetInt($"{ID}.size", SizeLevel);
        // data.SetInt($"{ID}.speed", SpeedLevel);
    }

    public void Load(SaveData data)
    {
        // implement when your SaveData shape is ready
        // Unlocked = data.GetInt($"{ID}.unlocked") == 1;
        // SizeLevel = data.GetInt($"{ID}.size");
        // SpeedLevel = data.GetInt($"{ID}.speed");
    }
}