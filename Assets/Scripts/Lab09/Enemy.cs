using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyLab09 : MonoBehaviour
{
	public Transform player;

	public Slider hpSlider;
	public int maxHealth = 100;

	[Header("Detect / Combat")]
	public float radius = 10f;
	public float health = 100f;
	public int damagePerProjectile = 20;

	[Header("NavMesh")]
	public NavMeshAgent agent;
	public float speed = 3.5f;
	public float stoppingDistance = 2.0f;

	[Header("Patrol")]
	public Transform[] waypoints;
	public float waypointReachDist = 0.7f;

	[Header("Shooting")]
	public float fireInterval = 5f;
	public GameObject projectilePrefab;
	public Transform firePoint;
	public float projectileSpeed = 15f;

	[Header("Raycast Shoot")]
	public float shootRange = 30f;
	public float shotLineDuration = 0.08f;
	public float shotLineStartWidth = 0.03f;
	public float shotLineEndWidth = 0.01f;

	private float fireTimer = 0f;
	private int wpIndex = 0;

	void Awake()
	{
		if (agent == null)
			agent = GetComponent<NavMeshAgent>();
	}

	void Start()
	{
		health = maxHealth;

		if (hpSlider != null)
		{
			hpSlider.minValue = 0;
			hpSlider.maxValue = maxHealth;
			hpSlider.value = health;
		}

		if (agent != null)
		{
			agent.speed = speed;
			agent.stoppingDistance = stoppingDistance;
			agent.updateRotation = false; // we rotate manually to face player / movement
		}

		StartPatrol();
	}

	void Update()
	{
		if (player == null)
			return;

		if (hpSlider != null)
			hpSlider.value = health;

		if (hpSlider != null && Camera.main != null)
			hpSlider.transform.parent.LookAt(Camera.main.transform);

		Vector3 toPlayer = player.position - transform.position;
		float dist = toPlayer.magnitude;

		if (dist <= radius)
		{
			// chase
			if (agent != null && agent.enabled)
			{
				agent.isStopped = false;
				agent.SetDestination(player.position);
			}

			// face player (flat)
			Vector3 flatDir = new Vector3(toPlayer.x, 0f, toPlayer.z);
			if (flatDir.sqrMagnitude > 0.001f)
			{
				Quaternion targetRot = Quaternion.LookRotation(flatDir);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
			}

			// shoot
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
			Patrol();
		}
	}

	void StartPatrol()
	{
		if (agent == null || !agent.enabled)
			return;

		if (waypoints == null || waypoints.Length == 0)
			return;

		wpIndex = Mathf.Clamp(wpIndex, 0, waypoints.Length - 1);
		agent.SetDestination(waypoints[wpIndex].position);
	}

	void Patrol()
	{
		if (agent == null || !agent.enabled)
			return;

		if (waypoints == null || waypoints.Length < 3)
			return; // lab wants >= 3

		if (!agent.pathPending && agent.remainingDistance <= waypointReachDist)
		{
			wpIndex = (wpIndex + 1) % waypoints.Length;
			agent.SetDestination(waypoints[wpIndex].position);
		}

		// face movement direction a bit (optional, feels nicer)
		Vector3 v = agent.velocity;
		v.y = 0f;
		if (v.sqrMagnitude > 0.1f)
		{
			Quaternion targetRot = Quaternion.LookRotation(v);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 8f * Time.deltaTime);
		}
	}

	void Shoot(Vector3 dir)
	{
		if (firePoint == null)
			return;

		Vector3 origin = firePoint.position;

		// Keep it horizontal so it doesn't shoot into the ground
		dir.y = 0f;
		dir = dir.normalized;

		if (Physics.Raycast(origin, dir, out RaycastHit hit, shootRange, ~0, QueryTriggerInteraction.Ignore))
		{
			ShowRay(origin, hit.point);

			PlayerLab09 p = hit.collider.GetComponentInParent<PlayerLab09>();
			if (p != null)
			{
				p.health -= damagePerProjectile;
			}
		}
		else
		{
			ShowRay(origin, origin + dir * shootRange);
		}
	}
	void ShowRay(Vector3 start, Vector3 end)
	{
		GameObject go = new GameObject("EnemyShotRay");
		LineRenderer lr = go.AddComponent<LineRenderer>();

		lr.positionCount = 2;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);

		lr.startWidth = shotLineStartWidth;
		lr.endWidth = shotLineEndWidth;
		lr.material = new Material(Shader.Find("Sprites/Default"));
		lr.startColor = Color.white;
		lr.endColor = Color.white;

		Destroy(go, shotLineDuration);
	}


	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}