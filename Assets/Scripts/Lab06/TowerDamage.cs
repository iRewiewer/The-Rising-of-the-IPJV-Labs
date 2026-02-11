using UnityEngine;

public class TowerDamageZone : MonoBehaviour
{
	public float pushBackDistance = 5f;

	public float hitCooldown = 0.5f;
	private float lastHitTime = -999f;

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;

		if (Time.time - lastHitTime < hitCooldown)
			return;

		lastHitTime = Time.time;

		PlayerLab06 player = other.GetComponent<PlayerLab06>();

		player.health -= 1;

		if(player.health < 0)
		{
			player.messages.text = "You're already dead bro\nstop";
		}

		CharacterController controller = other.GetComponent<CharacterController>();
		
		Vector3 dir = (other.transform.position - transform.position).normalized;
		dir.y = 0f;

		controller.Move(dir * pushBackDistance);
	}
}
