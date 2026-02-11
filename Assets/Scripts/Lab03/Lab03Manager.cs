using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Zone { Red, Blue, Green }

public class GameManager : MonoBehaviour
{
	[Header("Buttons")]
	public GameObject spawnRedBtn;
	public GameObject killRedBtn;
	public GameObject spawnBlueBtn;
	public GameObject killBlueBtn;
	public GameObject spawnGreenBtn;
	public GameObject killGreenBtn;

	[Header("Zones")]
	public Transform redZone;
	public Transform blueZone;
	public Transform greenZone;

	[Header("Spawn")]
	public GameObject prefab;

	private Dictionary<Zone, Transform> zoneRoot;
	private Dictionary<Zone, List<GameObject>> spawned;

	void Awake()
	{
		zoneRoot = new Dictionary<Zone, Transform>
		{
			{ Zone.Red,   redZone },
			{ Zone.Blue,  blueZone },
			{ Zone.Green, greenZone },
		};

		spawned = new Dictionary<Zone, List<GameObject>>
		{
			{ Zone.Red,   new List<GameObject>() },
			{ Zone.Blue,  new List<GameObject>() },
			{ Zone.Green, new List<GameObject>() },
		};

		spawnRedBtn.GetComponent<Button>().onClick.AddListener(() => SpawnOn(Zone.Red));
		killRedBtn.GetComponent<Button>().onClick.AddListener(() => KillAll(Zone.Red));

		spawnBlueBtn.GetComponent<Button>().onClick.AddListener(() => SpawnOn(Zone.Blue));
		killBlueBtn.GetComponent<Button>().onClick.AddListener(() => KillAll(Zone.Blue));

		spawnGreenBtn.GetComponent<Button>().onClick.AddListener(() => SpawnOn(Zone.Green));
		killGreenBtn.GetComponent<Button>().onClick.AddListener(() => KillAll(Zone.Green));
	}

	void SpawnOn(Zone zone)
	{
		if (prefab == null)
		{
			return;
		}

		Transform _zone = zoneRoot[zone];
		if (_zone == null)
		{
			return;
		}

		Vector3 pos = RandomPointOnTop(_zone);
		if (zone == Zone.Blue)
		{
			pos.y = 10;
		}

		GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
		spawned[zone].Add(obj);
		ZoneBehavior behavior = obj.AddComponent<ZoneBehavior>();
		behavior.zone = zone;
	}

	void KillAll(Zone zone)
	{
		List<GameObject> list = spawned[zone];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != null)
			{
				Destroy(list[i]);
			}
		}
		list.Clear();
	}

	static Vector3 RandomPointOnTop(Transform target)
	{
		target.TryGetComponent(out Renderer rend);
		Bounds b = rend.bounds;
		float x = Random.Range(b.min.x, b.max.x);
		float z = Random.Range(b.min.z, b.max.z);
		float y = Random.Range(2, 50);
		return new Vector3(x, y, z);
	}
}
