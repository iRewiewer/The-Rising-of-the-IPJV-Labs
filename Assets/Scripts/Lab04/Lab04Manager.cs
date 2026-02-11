using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lab04Manager : MonoBehaviour
{
	[SerializeField] private GameObject summonBtn;
	[SerializeField] private GameObject incBtn;
	[SerializeField] private GameObject subBtn;
	[SerializeField] private GameObject ball;
	[SerializeField] private Transform spawnZone;
	[SerializeField] private AchievementToast toast;
	[SerializeField] private Animator mascot;
	[SerializeField] private Animator mascot2;
	private static int ScoreParam = Animator.StringToHash("score");

	public TMP_Text scoreLabel;
	private int score = 0;
	public enum Lab04Team { Blue, Red }

	private bool shownAchievement1 = false;
	private bool shownAchievement2 = false;

	public static Lab04Manager Instance { get; private set; }

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		summonBtn.GetComponent<Button>().onClick.AddListener(() => Summon());
		incBtn.GetComponent<Button>().onClick.AddListener(() => IncScore());
		subBtn.GetComponent<Button>().onClick.AddListener(() => SubScore());
		scoreLabel.text = $"Score: {score}";
	}

	private void Summon()
	{
		BoxCollider box = spawnZone.GetComponent<BoxCollider>();
		
		// Random point within box (local space)
		Vector3 localPos = new Vector3(
			Random.Range(-0.5f, 0.5f) * box.size.x,
			Random.Range(-0.5f, 0.5f) * box.size.y,
			Random.Range(-0.5f, 0.5f) * box.size.z
		);

		Vector3 worldPos = box.transform.TransformPoint(localPos + box.center);

		GameObject obj = Instantiate(ball, worldPos, Quaternion.identity);
		Rigidbody rb = obj.GetComponent<Rigidbody>();

		rb.AddForce(Random.onUnitSphere * Random.Range(2.0f, 5.0f), ForceMode.Impulse);
	}

	private void IncScore()
	{
		scoreLabel.text = $"Score: {++score}";
		mascot.SetInteger(ScoreParam, score);
		mascot2.SetInteger(ScoreParam, score);
	}

	private void SubScore()
	{
		scoreLabel.text = $"Score: {--score}";
		mascot.SetInteger(ScoreParam, score);
		mascot2.SetInteger(ScoreParam, score);
	}

	public void AddScore(Lab04Team team)
	{
		if(team == Lab04Team.Red)
		{
			IncScore();
		}
		else
		{
			SubScore();
		}

		if(score >= 10 && !shownAchievement1)
		{
			shownAchievement1 = true;
			toast.Show("Achievement:\nGetting Warmed Up");
		}

		if (score >= 20 && !shownAchievement2)
		{
			shownAchievement2 = true;
			toast.Show("Achievement Get!\nOn Fire!");
		}
	}
}
