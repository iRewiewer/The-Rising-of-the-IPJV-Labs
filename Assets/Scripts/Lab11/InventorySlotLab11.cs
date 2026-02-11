using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotLab11 : MonoBehaviour
{
	[Header("UI")]
	public Button slotButton;          // click slot = use
	public Button deleteButton;        // X button = remove
	public Image iconImage;
	public TMP_Text nameText;

	private ItemLab11 item;
	private PlayerStatsLab11 playerStats;

	private void Awake()
	{
		if (slotButton != null)
			slotButton.onClick.AddListener(OnUsePressed);

		if (deleteButton != null)
			deleteButton.onClick.AddListener(OnRemovePressed);
	}

	public void Set(ItemLab11 newItem, PlayerStatsLab11 stats)
	{
		item = newItem;
		playerStats = stats;

		if (iconImage != null)
		{
			iconImage.sprite = item.icon;
			iconImage.enabled = (iconImage.sprite != null);
		}

		if (nameText != null)
			nameText.text = item.itemName;

		if (slotButton != null) slotButton.interactable = true;
		if (deleteButton != null) deleteButton.interactable = true;
	}

	public void Clear()
	{
		item = null;
		playerStats = null;

		if (iconImage != null)
		{
			iconImage.sprite = null;
			iconImage.enabled = false;
		}

		if (nameText != null)
			nameText.text = "";

		if (slotButton != null) slotButton.interactable = false;
		if (deleteButton != null) deleteButton.interactable = false;
	}

	public void OnUsePressed()
	{
		if (item == null || playerStats == null)
			return;

		item.Use(playerStats);

		// If the item is consumable, it removes itself via InventoryManager,
		// which triggers UI refresh.
		// If it's equipable, it toggles stats and stays in inventory.
	}

	public void OnRemovePressed()
	{
		if (item == null)
			return;

		InventoryManagerLab11.Instance.Remove(item);
	}
}
