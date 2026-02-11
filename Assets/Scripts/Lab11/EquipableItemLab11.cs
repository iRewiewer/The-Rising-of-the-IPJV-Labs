using UnityEngine;

[CreateAssetMenu(fileName = "New Equipable", menuName = "Lab11/Equipable")]
public class EquipableItemLab11 : ItemLab11
{
	public int strengthBonus = 65;
	public int dexterityBonus = 85;

	[HideInInspector] public bool isEquipped = false;

	public override void Use(PlayerStatsLab11 player)
	{
		if (!isEquipped)
		{
			player.ApplyEquipBonus(strengthBonus, dexterityBonus);
			isEquipped = true;
		}
		else
		{
			player.ApplyEquipBonus(5, 5);
			isEquipped = false;
		}
	}
}
