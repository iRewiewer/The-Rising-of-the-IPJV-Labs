using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[Header("UI")]
	public TMP_InputField nameInput;
	public TMP_Dropdown difficultyDropdown; // 0 Easy, 1 Normal, 2 Hard
	public TMP_Text statusText;

	private void Start()
	{
		EnsureGameState();

		// Optional: prefill from current GameState (nice UX)
		nameInput.text = GameStateLab10.Instance.playerName;

		int diff = (int)GameStateLab10.Instance.difficulty;
		if (diff >= 0 && diff < difficultyDropdown.options.Count)
			difficultyDropdown.value = diff;

		difficultyDropdown.RefreshShownValue();

		SetStatus("");
	}

	public void OnStartNewGamePressed()
	{
		EnsureGameState();

		ApplyUiToState();

		// Defaults for new game
		GameStateLab10.Instance.health = 50;  // match your Player default
		GameStateLab10.Instance.xp = 0;

		SceneManager.LoadScene($"Scenes/Lab10_1");
	}

	public void OnLoadGamePressed()
	{
		EnsureGameState();

		bool loaded = GameStateLab10.Instance.Load();
		if (!loaded)
		{
			SetStatus("No save found.");
			return;
		}

		// After load, ensure difficulty/name UI reflects save (optional)
		nameInput.text = GameStateLab10.Instance.playerName;
		difficultyDropdown.value = (int)GameStateLab10.Instance.difficulty;
		difficultyDropdown.RefreshShownValue();

		SetStatus("Loaded save.");
		SceneManager.LoadScene($"Scenes/Lab10_1");
	}

	public void OnQuitPressed()
	{
		Application.Quit();
	}

	private void ApplyUiToState()
	{
		string playerName = nameInput.text.Trim();
		if (string.IsNullOrEmpty(playerName))
			playerName = "Player";

		GameStateLab10.Instance.playerName = playerName;
		GameStateLab10.Instance.difficulty = (DifficultyLab10)difficultyDropdown.value;
	}

	private void EnsureGameState()
	{
		if (GameStateLab10.Instance != null)
			return;

		GameObject go = new GameObject("GameState");
		go.AddComponent<GameStateLab10>();
	}

	private void SetStatus(string msg)
	{
		if (statusText != null)
			statusText.text = msg;
	}
}
