using UnityEngine;

public class PlayerProjectileLab09 : MonoBehaviour
{
	public int damage = 5;
	public float lifeTime = 10f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		EnemyLab09 enemy = other.GetComponent<EnemyLab09>();
		if (enemy == null) return;
		enemy.health -= damage;
		Destroy(gameObject);
	}
}
