using UnityEngine;

public class FreeFlyCamera : MonoBehaviour
{
	public float moveSpeed = 40f;          // base movement speed
	public float boostMultiplier = 6f;    // hold Shift to boost
	public float mouseSensitivity = 1.5f;   // look speed

	private float yaw, pitch;
	private bool cursorUnlockedToggle = false; // user toggle state (F1/Esc/Click)
	private bool lastEffectiveUnlocked = false; // last applied lock state

	void Start()
	{
		var e = transform.eulerAngles;
		yaw = e.y;
		pitch = e.x;

		ApplyCursorLock(locked: true); // start locked
		lastEffectiveUnlocked = false;
	}

	void Update()
	{
		// --- Cursor lock logic ---
		bool altHeld = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

		// Toggle with F1, free with Esc, re-lock on LMB (if not holding Alt)
		if (Input.GetKeyDown(KeyCode.F1))
			cursorUnlockedToggle = !cursorUnlockedToggle;

		if (Input.GetKeyDown(KeyCode.Escape))
			cursorUnlockedToggle = true; // free

		if (Input.GetMouseButtonDown(0) && !altHeld)
			cursorUnlockedToggle = false; // lock back on click

		// Effective unlocked if Alt is held OR user toggled unlocked
		bool effectiveUnlocked = altHeld || cursorUnlockedToggle;

		// Only apply when state changes
		if (effectiveUnlocked != lastEffectiveUnlocked)
		{
			ApplyCursorLock(locked: !effectiveUnlocked);
			lastEffectiveUnlocked = effectiveUnlocked;
		}

		// --- Control only when locked ---
		if (!effectiveUnlocked)
		{
			HandleLook();
			HandleMovement();
		}

		// Mouse wheel adjusts base speed
		float scroll = Input.mouseScrollDelta.y;
		if (Mathf.Abs(scroll) > 0.01f)
		{
			moveSpeed = Mathf.Max(0.1f, moveSpeed * (1f + scroll * 0.1f));
		}
	}

	void HandleLook()
	{
		yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
		pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		pitch = Mathf.Clamp(pitch, -89f, 89f);
		transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
	}

	void HandleMovement()
	{
		float x = Input.GetAxisRaw("Horizontal"); // A/D
		float z = Input.GetAxisRaw("Vertical");   // W/S
		float y = 0f;

		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E)) y += 1f;          // up
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.Q)) y -= 1f;    // down

		Vector3 dir = new Vector3(x, y, z);
		if (dir.sqrMagnitude > 1f) dir.Normalize();

		float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1f);
		transform.position += transform.TransformDirection(dir) * speed * Time.deltaTime;
	}

	void ApplyCursorLock(bool locked)
	{
		Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !locked;
	}
}
