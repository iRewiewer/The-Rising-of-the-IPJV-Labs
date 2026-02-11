using UnityEngine;

public class PlayerLab07 : MonoBehaviour
{
	[Header("References")]
	public CharacterController controller;
	public Transform cam;
	public Transform model;
	public Animator animator;
	public Animator gunAnimator;
	public Transform firePoint;

	[Header("Stats")]
	public int health = 50;
	public int damage = 5;
	public float shootRange = 100f;

	[Header("Movement")]
	public float speed = 20f;
	public float sprintSpeed = 9f;
	public float gravity = -20f;

	[Header("Camera")]
	public float mouseSensitivity = 2f;
	public float camDistance = 10f;
	public float camHeight = 4f;

	[Header("Shot Visuals")]
	public float shotLineDuration = 0.1f;
	public float shotLineStartWidth = 0.03f;
	public float shotLineEndWidth = 0.01f;

	private float yaw;
	private float pitch;
	private float yVel;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		yaw = transform.eulerAngles.y;
	}

	void Update()
	{
		// Shooting
		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}

		// Camera + player rotation
		pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

		cam.rotation = Quaternion.Euler(pitch, yaw, 0f);
		transform.rotation = Quaternion.Euler(0f, yaw, 0f);
		cam.position = transform.position + Vector3.up * camHeight - cam.forward * camDistance;

		// Movement input
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		Vector3 fb = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;
		Vector3 lr = new Vector3(cam.right.x, 0f, cam.right.z).normalized;

		float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;
		Vector3 movement = (fb * v + lr * h).normalized * currentSpeed;

		// Animations
		bool moving = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude > 0.1f;
		bool sprinting = Input.GetKey(KeyCode.LeftShift);

		animator.SetBool("isWalking", moving && !sprinting);
		animator.SetBool("isRunning", moving && sprinting);

		// Model rotation
		Vector3 axisFix = new Vector3(-90f, 0f, 0f);
		if (movement.sqrMagnitude > 0.001f)
		{
			model.rotation = Quaternion.LookRotation(movement, Vector3.up) * Quaternion.Euler(axisFix);
		}
		else
		{
			model.rotation = transform.rotation * Quaternion.Euler(axisFix);
		}

		// Gravity
		if (controller.isGrounded)
		{
			if (yVel < 0f)
				yVel = -1f;
		}
		else
		{
			yVel += gravity * Time.deltaTime;
		}

		// Final move
		Vector3 velocity = movement;
		velocity.y = yVel;

		controller.Move(velocity * Time.deltaTime);
	}

	void Shoot()
	{
		if (gunAnimator != null)
		{
			gunAnimator.SetTrigger("Shoot");
		}

		if (firePoint == null)
			return;

		Vector3 origin = firePoint.position;
		Vector3 direction = firePoint.forward;

		if (Physics.Raycast(origin, direction, out RaycastHit hit, shootRange))
		{
			ShowRay(origin, hit.point);

			GuardTower tower = hit.collider.GetComponentInParent<GuardTower>();
			if (tower != null)
			{
				tower.health -= damage;

				if (tower.health <= 0)
				{
					Destroy(tower.gameObject);
				}
			}
		}
		else
		{
			ShowRay(origin, origin + direction * shootRange);
		}
	}

	void ShowRay(Vector3 start, Vector3 end)
	{
		GameObject go = new GameObject("ShotRay");
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
}
