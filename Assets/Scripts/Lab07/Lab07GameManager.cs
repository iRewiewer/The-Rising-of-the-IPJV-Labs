using TMPro;
using UnityEngine;

public class Lab07GameManager: MonoBehaviour
{
	public PlayerLab07 player;
	public GuardTower guardTower;
	public TMP_Text health_text;

	public GameObject health_panel;
	public GameObject gameOverPanel;

	void Start()
	{
		gameOverPanel.SetActive(false);
	}

	void Update()
	{
		health_text.text = $"Player Health: {player.health}\n" +
			$"Guard Tower Health: {guardTower.health}";

		if (Input.GetKeyDown(KeyCode.K))
		{
			player.gameObject.SetActive(true);
			player.health = 999;

			health_panel.SetActive(true);
			gameOverPanel.SetActive(false);

		}

		if (player.health <= 0)
		{
			player.gameObject.SetActive(false);
			health_panel.SetActive(false);
			gameOverPanel.SetActive(true);
			return;
		}
	}
}
