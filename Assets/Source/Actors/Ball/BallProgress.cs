public class BallProgress
{
    static string ID =>  "ball";
    
    static string KeyDamage => $"{ID}_damage";

    public static int GetDamage(SaveData data) => data.GetInt(KeyDamage);
    public static void SetDamage(SaveData data, int damage) => data.SetInt(KeyDamage, damage);
}