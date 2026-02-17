using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Auto-Paddle Unlock")]
public class AutoPaddleUnlock : ShopUpgrade
{
	[SerializeField] AutoPaddleProgress paddleProgress;

	public override int CurrLvl => paddleProgress.Unlocked ? 1 : 0;

	protected override void UpgradeLogic() => paddleProgress.Unlock(); 
	public override int GetCost() => baseCost;
}