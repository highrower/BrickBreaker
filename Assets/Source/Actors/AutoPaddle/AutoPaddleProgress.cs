using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Progress/Auto Paddle Progress")]

public class AutoPaddleProgress : ScriptableObject, ISaveable
{
    public string ID => "auto_paddle";
    public bool unlocked;
    public int sizeLevel;
    public int speedLevel;
    public event Action OnUnlocked;
    
    string KeyUnlocked => $"{ID}_unlocked";
    string KeySpeed    => $"{ID}_speed";
    string KeySize     => $"{ID}_size";

    
    public void Unlock()
    {
        if (unlocked) return;
        unlocked = true;

        OnUnlocked?.Invoke();
    }

    public void Save(SaveData data)
    {
        data.UpgradeIdToLevel[KeyUnlocked] = unlocked ? 1 : 0;
        data.UpgradeIdToLevel[KeySpeed] = speedLevel;
        data.UpgradeIdToLevel[KeySize] = sizeLevel;
    }

    public void Load(SaveData data)
    {
        if (data.UpgradeIdToLevel.TryGetValue(KeyUnlocked, out var unlockedInt) && unlockedInt == 1)
            Unlock();
        if (data.UpgradeIdToLevel.TryGetValue(KeySpeed, out var s)) 
            speedLevel = s;
        if (data.UpgradeIdToLevel.TryGetValue(KeySize, out var z)) 
            sizeLevel = z;
    }
}