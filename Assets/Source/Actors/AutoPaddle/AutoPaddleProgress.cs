using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Progress/Auto Paddle Progress")]

public class AutoPaddleProgress : ScriptableObject, ISaveable
{
    public string ID => "auto_paddle";
    public bool Unlocked;
    public int SizeLevel { get; }
    public int SpeedLevel;
    public event Action OnUnlocked;
    
    public void Unlock()
    {
        if (Unlocked) return;
        Unlocked = true;

        OnUnlocked?.Invoke();
    }

    public void Save(SaveData data) { /* later */ }
    public void Load(SaveData data) { /* later */ }
}