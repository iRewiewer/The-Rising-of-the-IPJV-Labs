using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Lab11/Consumable")]
public class ConsumableItemLab11 : ItemLab11
{
	public int healthGain = 250;

	public override void Use(PlayerStatsLab11 player)
	{
		player.Heal(healthGain);
		InventoryManagerLab11.Instance.Remove(this);
	}
}
