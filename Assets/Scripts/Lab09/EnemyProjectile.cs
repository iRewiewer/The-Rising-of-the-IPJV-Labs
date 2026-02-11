using UnityEngine;

public class EnemyProjectileLab09 : MonoBehaviour
{
	public int damage = 20;
	public float lifeTime = 5f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerLab09 player = other.GetComponent<PlayerLab09>();
		if (player == null) return;
		player.health -= damage;
		Destroy(gameObject);
	}

	private void OnCollisionEnter(Collision other)
	{
		PlayerLab09 player = other.gameObject.GetComponent<PlayerLab09>();
		if (player == null) return;
		player.health -= damage;
		Destroy(gameObject);
	}
}
