using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Ball Damage Upgrade")]
public class BallDamageUpgrade : ShopItem {
	[SerializeField] private BallSettings ballSettings;
	[SerializeField] private int          damageIncrease = 1;

	protected override int CurrentLevel => GameProgress.DamageLevel;

	public override void ApplyUpgrade() {
		if (!IsMaxed)
			GameProgress.DamageLevel++;
	}

	public override int GetCost() => (GameProgress.DamageLevel + 1) * baseCost;
}