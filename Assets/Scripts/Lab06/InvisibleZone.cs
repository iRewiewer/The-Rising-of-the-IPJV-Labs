using UnityEngine;

public class InvisibleZone : MonoBehaviour
{
	public bool playerInside = false;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			playerInside = true;
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
			playerInside = false;
	}
}
