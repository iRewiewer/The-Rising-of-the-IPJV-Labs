using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerLab10 : MonoBehaviour
{
	[Header("References")]
	public CharacterController controller;
	public Transform cam;
	public Transform model;
	public List<Transform> playerMeshes;
	public Animator animator;
	public Animator gunAnimator;
	public Transform firePoint;

	[Header("Person Type")]
	public int viewState = 0; // 0 - fps, 1 - 3rd person, 2 - top down
	public float fpsCamHeight = 0.5f;
	public float tpsCamHeight = 1.6f;
	public float tpsCamDistance = 4.0f;

	[Header("FPS")]
	public Transform rig;
	public Vector3 fpsRigWorldOffset = Vector3.zero;

	[Header("TDS")]
	public Camera tdsCamera;
	public LayerMask groundMask = ~0;

	[Header("Ammo")]
	public int maxAmmo = 5;
	public int ammo;

	[Header("Gun Settings")]
	public Transform gun;
	private Vector3 fpsGunLocalPos = new Vector3(0.00362f, -0.0081f, 0.00559f);
	private Vector3 fpsGunLocalRot = new Vector3(20.281f, -10.986f, -1.049f);
	private Vector3 fpsGunLocalScale = new Vector3(0.01f, 0.01f, 0.01f);

	private Vector3 tpsGunLocalPos = new Vector3(-0.00083f, -0.00484f, -0.00257f);
	private Vector3 tpsGunLocalRot = new Vector3(100.536f, 76.537f, 75.423f);
	private Vector3 tpsGunLocalScale = new Vector3(0.012f, 0.012f, 0.012f);

	private Vector3 tdsGunLocalPos = new Vector3(-0.00083f, -0.00484f, -0.00257f);
	private Vector3 tdsGunLocalRot = new Vector3(100.536f, 76.537f, 75.423f);
	private Vector3 tdsGunLocalScale = new Vector3(0.012f, 0.012f, 0.012f);

	[Header("Stats")]
	public int health = 50;
	public int damage = 5;
	public int xp = 0;
	public string playerName = string.Empty;
	public float shootRange = 100f;

	[Header("Movement")]
	public float speed = 10f;
	public float sprintSpeed = 20f;
	public float gravity = -20f;

	[Header("Camera")]
	public float mouseSensitivity = 2f;
	public float pitchMin = -80f;
	public float pitchMax = 80f;

	[Header("Shot Visuals")]
	public float shotLineDuration = 0.1f;
	public float shotLineStartWidth = 0.03f;
	public float shotLineEndWidth = 0.01f;

	[Header("Corsshair")]
	public Image crosshairImage;
	public Sprite crosshairA;
	public Sprite crosshairB;

	[Header("Navmesh")]
	public NavMeshAgent agent;

	private float yaw;
	private float pitch;
	private float yVel;
	private bool useAltCrosshair = false;

	void Awake()
	{
	}

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		yaw = transform.eulerAngles.y;

		ammo = maxAmmo;
		viewState = 0;

		SetGunTransform();
		SetPlayerMeshesVisible();

		if (GameStateLab10.Instance != null)
		{
			playerName = GameStateLab10.Instance.playerName;
			health = GameStateLab10.Instance.health;
			xp = GameStateLab10.Instance.xp;
		}
	}

	void Update()
	{
		// Shooting
		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}

		// Camera + player rotation
		if (viewState != 2)
		{
			pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
			yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

			pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

			transform.rotation = Quaternion.Euler(0f, yaw, 0f);
			cam.rotation = Quaternion.Euler(pitch, yaw, 0f);
		}

		UpdateCursorForView();

		switch (viewState)
		{
			case 0:
			{
				cam.position = transform.position + Vector3.up * fpsCamHeight;
				break;
			}
			case 1:
			{
				cam.position = transform.position + Vector3.up * tpsCamHeight - cam.forward * tpsCamDistance;
				break;
			}
			case 2:
			{
				Vector3 offset = new Vector3(0f, 12f, -6f);
				cam.position = transform.position + offset;
				cam.rotation = Quaternion.Euler(60f, 0f, 0f);
				break;
			}
		}

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

		if (viewState != 2)
		{
			controller.Move(velocity * Time.deltaTime);
		}

		if (viewState == 2)
		{
			HandleTdsClickMove();

			// rotate model in movement direction
			if (agent.velocity.sqrMagnitude > 0.1f)
			{
				Vector3 dir = agent.velocity;
				dir.y = 0f;
				model.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(-90f, 0f, 0f);
			}
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			useAltCrosshair = !useAltCrosshair;
			crosshairImage.sprite = useAltCrosshair ? crosshairB : crosshairA;

			Debug.Log(crosshairImage.transform.position);

			int width = Screen.width;
			int height = Screen.height;

			if (useAltCrosshair)
			{
				crosshairImage.transform.position = new Vector3(width / 2, height / 2);
				crosshairImage.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
				crosshairImage.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
			else
			{
				crosshairImage.transform.position = new Vector3(1222, 771, 0);
				crosshairImage.transform.localScale = new Vector3(5, 5, 1);
				crosshairImage.transform.localRotation = Quaternion.Euler(0f, 0f, 86.93f);
			}
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			viewState = (viewState + 1) % 3;
			PersonSwitcher();
		}

		if(ammo == 0 && Input.GetKeyDown(KeyCode.R))
		{
			ammo = maxAmmo;
		}

		crosshairImage.enabled = viewState == 2 ? false : true;
	}
	void LateUpdate()
	{
		if (viewState != 0)
			return;

		if (rig == null || cam == null)
			return;

		rig.position = cam.position + fpsRigWorldOffset;
		rig.rotation = cam.rotation;
	}
	void HandleTdsClickMove()
	{
		if (!Input.GetMouseButtonDown(1))
			return;

		Ray ray = tdsCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, 2000f, groundMask, QueryTriggerInteraction.Ignore))
		{
			agent.isStopped = false;
			agent.SetDestination(hit.point);
		}
	}

	void PersonSwitcher()
	{
		if (viewState == 2)
		{
			agent.enabled = true;
			agent.Warp(transform.position);
		}
		else
		{
			agent.ResetPath();
			agent.enabled = false;
		}

		SetPlayerMeshesVisible();
		SetGunTransform();
	}

	void SetGunTransform()
	{
		if (gun != null)
		{
			switch(viewState)
			{
				case 0:
				{
					gun.localPosition = fpsGunLocalPos;
					gun.localRotation = Quaternion.Euler(fpsGunLocalRot);
					gun.localScale = fpsGunLocalScale;
					break;
				}
				case 1:
				{
					gun.localPosition = tpsGunLocalPos;
					gun.localRotation = Quaternion.Euler(tpsGunLocalRot);
					gun.localScale = tpsGunLocalScale;
					break;
				}
				case 2:
				{
					gun.localPosition = tdsGunLocalPos;
					gun.localRotation = Quaternion.Euler(tdsGunLocalRot);
					gun.localScale = tdsGunLocalScale;
					break;
				}
			}
		}
	}
	void UpdateCursorForView()
	{
		if (viewState == 2)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void SetPlayerMeshesVisible()
	{
		bool visible = viewState != 0;

		foreach (Transform t in playerMeshes)
		{
			if (t != null)
			{
				t.gameObject.SetActive(visible);
			}
		}
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
		Vector3 direction;

		if (viewState == 2)
		{
			// shoot horizontally
			Vector3 fwd = model != null ? model.forward : transform.forward;
			direction = Vector3.ProjectOnPlane(fwd, Vector3.up).normalized;

			if (direction.sqrMagnitude < 0.0001f)
				direction = transform.forward;
		}
		else
		{
			direction = cam.forward;
		}

		if (ammo <= 0)
		{
			return;
		}

		ammo--;

		if (Physics.Raycast(origin, direction, out RaycastHit hit, shootRange))
		{
			ShowRay(origin, hit.point);

			EnemyLab10 ememy = hit.collider.GetComponentInParent<EnemyLab10>();
			if (ememy != null)
			{
				ememy.health -= damage;

				if (ememy.health <= 0)
				{
					xp += GameStateLab10.Instance.EnemyXpMultiplier();
					Destroy(ememy.gameObject);
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
