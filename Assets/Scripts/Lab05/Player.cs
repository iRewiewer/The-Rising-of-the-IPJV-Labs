using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLab05 : MonoBehaviour
{
	public CharacterController controller;
	public Transform cam;
	public Transform model;
	public Animator animator;

	public float speed = 20f;
	public float sprintSpeed = 9f;
	public float jumpHeight = 2f;

	public float gravity = -20f;
	public float mouseSensitivity = 2f;
	public float camDistance = 10f;
	public float camHeight = 4f;

	private float yaw, pitch, yVel;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		yaw = transform.eulerAngles.y;
	}

	void Update()
	{
		pitch = pitch - Input.GetAxis("Mouse Y") * mouseSensitivity;
		yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

		cam.rotation = Quaternion.Euler(pitch, yaw, 0f);
		cam.position = transform.position + Vector3.up * camHeight - cam.forward * camDistance;

		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		Vector3 fb = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;
		Vector3 lr = new Vector3(cam.right.x, 0, cam.right.z).normalized;

		float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

		Vector3 movement = (fb * v + lr * h).normalized * currentSpeed;

		bool moving = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude > 0.1f;
		bool grounded = controller.isGrounded;

		animator.SetBool("isWalking", moving && !Input.GetKey(KeyCode.LeftShift));
		animator.SetBool("isRunning", moving && Input.GetKey(KeyCode.LeftShift));
		animator.SetBool("isJumping", !grounded);

		Vector3 axisFix = new Vector3(-90f, 0f, 0f);
		model.rotation = Quaternion.LookRotation(movement, Vector3.up) * Quaternion.Euler(axisFix);

		if (controller.isGrounded)
		{
			yVel = -1f;
			if (Input.GetKeyDown(KeyCode.Space))
			{
				yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
			}
		}
		else yVel += gravity * Time.deltaTime;

		controller.Move(new Vector3(movement.x, yVel, movement.z) * Time.deltaTime);
	}
}
