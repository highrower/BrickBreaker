using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Shop/Ball Damage Upgrade")]
public class BallDamageUpgrade : ShopUpgrade
{
	[SerializeField] BallProgress progress;
	[SerializeField] int          damageIncrease = 1;

	public override int CurrLvl => progress.DamageLevel;

	protected override void UpgradeLogic()
	{
		if (!IsMaxed())
			progress.DamageLevel++;
	}

	public override int GetCost() => (progress.DamageLevel + 1) * baseCost;
}

