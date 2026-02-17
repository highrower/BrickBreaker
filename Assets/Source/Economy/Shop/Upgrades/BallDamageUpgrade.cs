using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Shop/Ball Damage Upgrade")]
public class BallDamageUpgrade : ShopUpgrade
{
	[SerializeField] BallProgress progress;

	public override int CurrLvl => progress.damageLevel;

	protected override void UpgradeLogic()
	{
		if (!IsMaxed())
			progress.damageLevel++;
	}

	public override int GetCost() => (progress.damageLevel + 1) * baseCost;
}

