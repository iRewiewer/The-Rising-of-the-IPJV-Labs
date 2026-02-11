using UnityEngine;

public class EnemyProjectileLab10 : MonoBehaviour
{
	public int damage = 20;
	public float lifeTime = 5f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerLab10 player = other.GetComponent<PlayerLab10>();
		if (player == null) return;
		player.health -= damage;
		Destroy(gameObject);
	}

	private void OnCollisionEnter(Collision other)
	{
		PlayerLab10 player = other.gameObject.GetComponent<PlayerLab10>();
		if (player == null) return;
		player.health -= damage;
		Destroy(gameObject);
	}
}
