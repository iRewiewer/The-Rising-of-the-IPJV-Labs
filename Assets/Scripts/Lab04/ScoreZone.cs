using UnityEngine;
using static Lab04Manager;

public class ScoreZone : MonoBehaviour
{

	void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("lab04_ball")) return;

		Lab04Manager.Instance.AddScore(this.name.Contains("red") ? Lab04Team.Red : Lab04Team.Blue);
		Destroy(other.gameObject);
	}
}
