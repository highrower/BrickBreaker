using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Auto-Paddle Unlock")]
public class AutoPaddleUnlock : ShopUpgrade
{
	[SerializeField] AutoPaddleSettings paddleSettings;

	public override int CurrLvl => (int)paddleSettings.Unlocked;

	protected override void UpgradeLogic()
	{
		if (!IsMaxed)
			paddleSettings.Unlocked = true;
	}

	public override int GetCost() => (paddleSettings.DamageLevel + 1) * baseCost;
}