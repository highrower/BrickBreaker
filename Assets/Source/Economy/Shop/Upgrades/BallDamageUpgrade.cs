using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Shop/Ball Damage Upgrade")]
public class BallDamageUpgrade : ShopUpgrade
{
	BallProgress progress;

	public override int CurrLvl => progress.DamageLevel;

	protected override void UpgradeLogic()
	{
		if (!IsMaxed())
			progress.DamageLevel++;
	}

	public override int GetCost() => (progress.DamageLevel + 1) * baseCost;
}

