using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Ball Damage Upgrade")]
public class BallDamageUpgrade : ShopItem {
	[SerializeField] BallSettings ballSettings;
	[SerializeField] int          damageIncrease = 1;

	protected override int CurrentLevel => ballSettings.DamageLevel;

	protected override void UpgradeLogic() {
		if (!IsMaxed)
			ballSettings.DamageLevel++;
	}

	public override int GetCost() => (ballSettings.DamageLevel + 1) * baseCost;
}