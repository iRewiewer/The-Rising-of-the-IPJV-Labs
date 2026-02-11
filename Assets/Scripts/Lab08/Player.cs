using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLab08 : MonoBehaviour
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
	public bool isFirstPerson = true;
	public float fpsCamHeight = 0.5f;
	public float tpsCamHeight = 1.6f;
	public float tpsCamDistance = 4.0f;

	[Header("Rig Follow (FPS)")]
	public Transform rig; // drag Player/Rig here
	public Vector3 fpsRigWorldOffset = Vector3.zero; // optional (usually zero)
	
	[Header("Ammo")]
	public int maxAmmo = 5;
	public int ammo;

	[Header("Gun Settings")]
	public Transform gun;
	public Vector3 fpsGunLocalPos = new Vector3(0.003523056f, -0.007309398f, 0.005011908f);
	public Vector3 fpsGunLocalRot = new Vector3(20.268f, 346.262f, 357.687f);
	public Vector3 fpsGunLocalScale = new Vector3(0.01f, 0.01f, 0.01f);

	public Vector3 tpsGunLocalPos = new Vector3(-0.0002799926f, -0.005460031f, -0.0006200004f);
	public Vector3 tpsGunLocalRot = new Vector3(83.826f, -90f, -90f);
	public Vector3 tpsGunLocalScale = new Vector3(0.01f, 0.01f, 0.01f);

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

	private float yaw;
	private float pitch;
	private float yVel;

	private bool useAltCrosshair = false;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		yaw = transform.eulerAngles.y;

		ammo = maxAmmo;

		SetGunTransform();
		SetPlayerMeshesVisible(!isFirstPerson);
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

		pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

		transform.rotation = Quaternion.Euler(0f, yaw, 0f);
		cam.rotation = Quaternion.Euler(pitch, yaw, 0f);

		if (isFirstPerson)
		{
			cam.position = transform.position + Vector3.up * fpsCamHeight;
		}
		else
		{
			cam.position =
				transform.position +
				Vector3.up * tpsCamHeight -
				cam.forward * tpsCamDistance;
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

		controller.Move(velocity * Time.deltaTime);

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
			PersonSwitcher();
		}

		if(ammo == 0 && Input.GetKeyDown(KeyCode.R))
		{
			ammo = maxAmmo;
		}
	}
	void LateUpdate()
	{
		if (!isFirstPerson)
			return;

		if (rig == null || cam == null)
			return;

		rig.position = cam.position + fpsRigWorldOffset;
		rig.rotation = cam.rotation;
	}

	void PersonSwitcher()
	{
		isFirstPerson = !isFirstPerson;
		SetPlayerMeshesVisible(!isFirstPerson);
		SetGunTransform();
	}
	void SetGunTransform()
	{
		if (gun != null)
		{
			if (isFirstPerson)
			{
				gun.localPosition = fpsGunLocalPos;
				gun.localRotation = Quaternion.Euler(fpsGunLocalRot);
				gun.localScale = fpsGunLocalScale;
			}
			else
			{
				gun.localPosition = tpsGunLocalPos;
				gun.localRotation = Quaternion.Euler(tpsGunLocalRot);
				gun.localScale = tpsGunLocalScale;
			}
		}
	}
	void SetPlayerMeshesVisible(bool visible)
	{
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
		Vector3 direction = cam.forward;

		if (ammo <= 0)
		{
			return;
		}

		ammo--;

		if (Physics.Raycast(origin, direction, out RaycastHit hit, shootRange))
		{
			ShowRay(origin, hit.point);

			GuardTowerLab08 tower = hit.collider.GetComponentInParent<GuardTowerLab08>();
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
