using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
	public int damage = 20;
	public float lifeTime = 5f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		PlayerLab07 player = other.GetComponent<PlayerLab07>();
		player.health -= damage;
		Destroy(gameObject);
	}

	private void OnCollisionEnter(Collision other)
	{
		PlayerLab07 player = other.gameObject.GetComponent<PlayerLab07>();
		player.health -= damage;
		Destroy(gameObject);
	}
}
