using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lab09GameManager: MonoBehaviour
{
	public PlayerLab09 player;
	public TMP_Text panelText;

	public GameObject info_panel;
	public GameObject gameOverPanel;
	public GameObject minimapUI;

	public Slider playerHpSlider;
	public int playerMaxHealth = 50;

	public Camera minimapCamera;

	private bool isMinimapLarge = false;

	void Start()
	{
		playerHpSlider.minValue = 0;
		playerHpSlider.maxValue = playerMaxHealth;

		gameOverPanel.SetActive(false);
	}

	void Update()
	{
		//minimapCamera.transform.position = new Vector3(player.transform.position.x, 60.0f, player.transform.position.z);

		if(Input.GetKeyDown(KeyCode.M))
		{
			RectTransform rt = minimapUI.GetComponent<RectTransform>();

			if (!isMinimapLarge)
			{
				rt.anchoredPosition = new Vector2(554f, 137f);
				rt.localScale = new Vector3(3f, 3f, 1f);
			}
			else
			{
				rt.anchoredPosition = new Vector2(820f, 420f);
				rt.localScale = new Vector3(1f, 1f, 1f);
			}

			isMinimapLarge = !isMinimapLarge;
		}

		panelText.text = $"Health: {player.health}\n" +
			$"Ammo: {player.ammo}/{player.maxAmmo}\n" +
			$"XP: {player.xp}";

		playerHpSlider.value = player.health;

		if (Input.GetKeyDown(KeyCode.K))
		{
			player.gameObject.SetActive(true);
			player.health = 999;
			player.ammo = 999;

			info_panel.SetActive(true);
			gameOverPanel.SetActive(false);
		}

		if (player.health <= 0)
		{
			player.gameObject.SetActive(false);
			info_panel.SetActive(false);
			gameOverPanel.SetActive(true);
			return;
		}
	}
}
