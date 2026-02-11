using UnityEngine;

public class PlayerProjectileLab08 : MonoBehaviour
{
	public int damage = 5;
	public float lifeTime = 10f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter(Collider other)
	{
		GuardTowerLab08 tower = other.GetComponent<GuardTowerLab08>();
		if (tower == null) return;
		tower.health -= damage;
		Destroy(gameObject);
	}
}
