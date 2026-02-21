using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Auto-Paddle Unlock")]
public class AutoPaddleUnlock : ShopUpgrade
{
	public override int GetLevel(SaveData data) => AutoPaddleProgress.GetUnlocked(data) ? 1 : 0;
	protected override void UpgradeLogic(SaveData data) =>
		AutoPaddleProgress.SetUnlocked(data, true); 
	public override int GetCost(SaveData data) => baseCost;
}