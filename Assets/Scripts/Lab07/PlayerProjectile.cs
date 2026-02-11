using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
	public int damage = 5;
	public float lifeTime = 10f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		GuardTower tower = other.GetComponent<GuardTower>();
		tower.health -= damage;
		Destroy(gameObject);
	}
}
