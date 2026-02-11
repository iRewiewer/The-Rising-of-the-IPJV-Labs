using UnityEngine;

public class ZoneBehavior : MonoBehaviour
{
	public Zone zone;

	private Vector3 startScale;
	private float scaleTimer;
	private float jumpTimer;
	private Rigidbody rb;

	void Start()
	{
		startScale = transform.localScale;

		if (zone == Zone.Blue)
		{
			rb = gameObject.AddComponent<Rigidbody>();
			rb.linearDamping = 0.2f;
			rb.freezeRotation = true;
		}
	}

	void Update()
	{
		switch (zone)
		{
			case Zone.Red:
				transform.Rotate(new Vector3(1, 0, 1) * 135f * Time.deltaTime);
				break;

			case Zone.Blue:
				HandleBlue();
				break;

			case Zone.Green:
				HandleGreen();
				break;
		}
	}

	void HandleBlue()
	{
		if (rb == null) return;

		jumpTimer += Time.deltaTime;
		if (jumpTimer >= 3.5f)
		{
			jumpTimer = 0f;
			rb.AddForce(Vector3.up * 16f, ForceMode.VelocityChange);
		}
	}

	void HandleGreen()
	{
		scaleTimer += Time.deltaTime * 2f;
		float scaleFactor = Mathf.Lerp(0.35f, 1.75f, (Mathf.Sin(scaleTimer) + 1f) / 2f); // float from 0 to 1
		transform.localScale = startScale * scaleFactor;
	}
}
