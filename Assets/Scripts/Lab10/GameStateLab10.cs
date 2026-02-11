using System.IO;
using UnityEngine;

public enum DifficultyLab10
{
	Easy = 0,
	Normal = 1,
	Hard = 2
}

public class GameStateLab10 : MonoBehaviour
{
	public static GameStateLab10 Instance;

	[Header("Session Data")]
	public string playerName = "Player";
	public DifficultyLab10 difficulty = DifficultyLab10.Normal;
	public int health = 50;
	public int xp = 0;

	private string SavePath => Path.Combine(Application.persistentDataPath, "SaveGameData.json");

	[System.Serializable]
	private class SaveData
	{
		public string playerName;
		public int difficulty;
		public int health;
		public int xp;
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public float EnemyHpMultiplier()
	{
		switch (difficulty)
		{
			case DifficultyLab10.Easy: return 0.75f;
			case DifficultyLab10.Hard: return 1.5f;
			default: return 1.0f;
		}
	}

	public int EnemyXpMultiplier()
	{
		switch (difficulty)
		{
			case DifficultyLab10.Easy: return 10;
			case DifficultyLab10.Hard: return 30;
			default: return 20;
		}
	}

	public void Save()
	{
		SaveData data = new SaveData
		{
			playerName = playerName,
			difficulty = (int)difficulty,
			health = health,
			xp = xp
		};

		File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
	}

	public bool Load()
	{
		if (!File.Exists(SavePath))
			return false;

		SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));

		playerName = string.IsNullOrEmpty(data.playerName) ? "Player" : data.playerName;
		difficulty = (DifficultyLab10)data.difficulty;
		health = data.health;
		xp = data.xp;

		return true;
	}
}
