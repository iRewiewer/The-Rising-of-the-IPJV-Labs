using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToMap2 : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		PlayerLab10 player = other.GetComponentInParent<PlayerLab10>();
		if (player == null)
			return;

		// Ensure GameState exists
		if (GameStateLab10.Instance == null)
		{
			GameObject go = new GameObject("GameState");
			go.AddComponent<GameStateLab10>();
		}

		// Copy current runtime data -> persistent state
		GameStateLab10.Instance.playerName = player.playerName; // or player.name if you kept that
		GameStateLab10.Instance.health = player.health;
		GameStateLab10.Instance.xp = player.xp;

		SceneManager.LoadScene("Scenes/Lab10_2");
	}
}
