using System;
public static class AutoPaddleProgress
{
    const string ID = "auto_paddle";
    static readonly string KeyUnlocked = $"{ID}_unlocked";
    static readonly string KeySpeed = $"{ID}_speed";
    static readonly string KeySize = $"{ID}_size";
    public static event Action OnUnlocked; 

    public static bool GetUnlocked(SaveData data) => data.GetBool01(KeyUnlocked);
    public static void SetUnlocked(SaveData data, bool v) => 
        data.SetBool01(KeyUnlocked, v, OnUnlocked);   

    public static int GetSpeedLevel(SaveData data) => data.GetInt(KeySpeed);
    public static void SetSpeedLevel(SaveData data, int v) => data.SetInt(KeySpeed, v);

    public static int GetSizeLevel(SaveData data) => data.GetInt(KeySize);
    public static void SetSizeLevel(SaveData data, int v) => data.SetInt(KeySize, v);
}