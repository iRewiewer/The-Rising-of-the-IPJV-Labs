using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerLab11 : MonoBehaviour
{
	public static InventoryManagerLab11 Instance;

	public event Action OnChanged;

	public int space = 12;
	public List<ItemLab11> items = new List<ItemLab11>();

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	public bool Add(ItemLab11 item)
	{
		if (items.Count >= space)
			return false;

		items.Add(item);
		OnChanged?.Invoke();
		return true;
	}

	public void Remove(ItemLab11 item)
	{
		items.Remove(item);
		OnChanged?.Invoke();
	}
}
