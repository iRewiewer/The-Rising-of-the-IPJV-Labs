using UnityEngine;

public class PlayerStatsLab11 : MonoBehaviour
{
	[Header("Attributes")]
	public int strength = 5;
	public int dexterity = 5;

	public void Heal(int amount)
	{
		PlayerLab10 p = GetComponent<PlayerLab10>();
		if (p != null)
			p.health += amount;
	}

	public void ApplyEquipBonus(int strDelta, int dexDelta)
	{
		strength = strDelta;
		dexterity = dexDelta;
	}
}
