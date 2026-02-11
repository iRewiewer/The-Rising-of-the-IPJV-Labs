using UnityEngine;

public class GuardTower : MonoBehaviour
{
	public Transform player;

	public float radius = 5f;
	public float health = 100f;
	public int damagePerProjectile = 20;

	public float fireInterval = 5f;
	public GameObject projectilePrefab;
	public Transform firePoint;
	public float projectileSpeed = 15f;

	private float fireTimer = 0f;

	void Update()
	{
		if (player == null)
			return;

		Vector3 toPlayer = player.position - transform.position;
		float dist = toPlayer.magnitude;

		if (dist <= radius)
		{
			Vector3 flatDir = new Vector3(toPlayer.x, 0f, toPlayer.z);
			if (flatDir.sqrMagnitude > 0.001f)
			{
				Quaternion targetRot = Quaternion.LookRotation(flatDir);
				transform.rotation = targetRot;
			}

			fireTimer -= Time.deltaTime;
			if (fireTimer <= 0f)
			{
				Vector3 dir = (player.position - firePoint.position).normalized;

				Shoot(dir);
				fireTimer = fireInterval;
			}
		}
		else
		{
			fireTimer = 0f;
		}
	}

	void Shoot(Vector3 dir)
	{
		if (projectilePrefab == null || firePoint == null)
			return;

		GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir));

		Rigidbody rb = projectile.GetComponent<Rigidbody>();
		rb.linearVelocity = dir * projectileSpeed;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
