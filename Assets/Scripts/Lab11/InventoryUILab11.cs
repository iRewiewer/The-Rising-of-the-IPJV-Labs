using TMPro;
using UnityEngine;

public class InventoryUILab11 : MonoBehaviour
{
	public GameObject inventoryPanel;
	public PlayerStatsLab11 playerStats;

	public TMP_Text statsText;

	private InventorySlotLab11[] slots;

	void Start()
	{
		slots = inventoryPanel.GetComponentsInChildren<InventorySlotLab11>(true);

		if (InventoryManagerLab11.Instance != null)
			InventoryManagerLab11.Instance.OnChanged += UpdateUI;

		inventoryPanel.SetActive(false);
		UpdateUI();
	}

	void Update()
	{
		statsText.text = "Stats\n" +
			$"Strength: {playerStats.strength}\n" +
			$"Dexterity: {playerStats.dexterity}";

		if (Input.GetKeyDown(KeyCode.I))
		{
			bool newState = !inventoryPanel.activeSelf;
			inventoryPanel.SetActive(newState);

			// optional: unlock mouse when inventory open
			Cursor.visible = newState;
			Cursor.lockState = newState ? CursorLockMode.None : CursorLockMode.Locked;
		}
	}

	void UpdateUI()
	{
		var inv = InventoryManagerLab11.Instance;
		if (inv == null) return;

		for (int i = 0; i < slots.Length; i++)
		{
			if (i < inv.items.Count) slots[i].Set(inv.items[i], playerStats);
			else slots[i].Clear();
		}
	}
}
