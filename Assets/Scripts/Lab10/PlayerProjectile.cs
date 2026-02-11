using UnityEngine;

public class PlayerProjectileLab10 : MonoBehaviour
{
	public int damage = 5;
	public float lifeTime = 10f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		EnemyLab10 enemy = other.GetComponent<EnemyLab10>();
		if (enemy == null) return;
		enemy.health -= damage;
		Destroy(gameObject);
	}
}
