using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Shop/Ball Damage Upgrade")]
public class BallDamageUpgrade : ShopUpgrade
{
	[SerializeField] BallProgress progress;

	public override int GetLevel(SaveData data) => BallProgress.GetDamage(data);

	protected override void UpgradeLogic(SaveData data) 
	{
		if (!IsMaxed(data))
			BallProgress.SetDamage(data, BallProgress.GetDamage(data) + 1);
	}

	public override int GetCost(SaveData data) => (GetLevel(data) + 1) * baseCost;
}

