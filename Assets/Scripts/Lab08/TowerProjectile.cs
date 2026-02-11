using UnityEngine;

public class TowerProjectileLab08 : MonoBehaviour
{
	public int damage = 20;
	public float lifeTime = 5f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerLab08 player = other.GetComponent<PlayerLab08>();
		if (player == null) return;
		player.health -= damage;
		Destroy(gameObject);
	}

	private void OnCollisionEnter(Collision other)
	{
		PlayerLab08 player = other.gameObject.GetComponent<PlayerLab08>();
		if (player == null) return;
		player.health -= damage;
		Destroy(gameObject);
	}
}
