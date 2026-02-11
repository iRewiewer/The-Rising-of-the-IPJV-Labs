using UnityEngine;

public abstract class ItemLab11 : ScriptableObject
{
	public string itemName;
	public Sprite icon;

	public abstract void Use(PlayerStatsLab11 player);
}
