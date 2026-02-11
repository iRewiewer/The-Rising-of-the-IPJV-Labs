using UnityEngine;

public class ItemGiverLab11 : MonoBehaviour
{
	public ItemLab11 itemToGive;

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerStatsLab11>() == null)
			return;

		if (InventoryManagerLab11.Instance.Add(itemToGive))
			Destroy(gameObject);
	}
}
