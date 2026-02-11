using System.Collections;
using System.Threading;
using UnityEngine;

public class ChestPickup : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;

		PlayerLab06 player = other.GetComponent<PlayerLab06>();
		
		player.health += 1;

		Destroy(gameObject);
	}
}
