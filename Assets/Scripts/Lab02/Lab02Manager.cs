using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Lab02Manager : MonoBehaviour
{
	[Header("Entity")]
	public GameObject Prefab;
	public string DisplayName = "Mr. Capsule";
	public Vector3 _SpawnPosition = new Vector3(491, 5, 23);
	public Material CapsuleMaterial;

	[Header("Miscellaneous")]
	[SerializeField] private Button spawnBtn;
	[SerializeField] private Button killBtn;
	[SerializeField] private Button activateBtn;
	[SerializeField] private Button deactivateBtn;
	public string TagToToggle = "Special";

	private List<GameObject> _spawnedEntities;

	void Awake()
	{
		if (Prefab == null)
		{
			Debug.LogError("Capsule prefab is not assigned!");
			return;
		}
		_spawnedEntities = new List<GameObject>();

		Prefab.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = DisplayName;

		spawnBtn.onClick.AddListener(SpawnEntity);
		killBtn.onClick.AddListener(KillEntity);
		activateBtn.onClick.AddListener(ActivateComponent);
		deactivateBtn.onClick.AddListener(DeactivateComponent);
	}

	void Update()
	{
		SceneChanger.SceneChangeOnFKey();
	}

	private void SpawnEntity()
	{
		Prefab.GetComponent<Renderer>().material = CapsuleMaterial;
		_spawnedEntities.Add(Instantiate(Prefab, _SpawnPosition, Quaternion.identity));
	}

	private void KillEntity()
	{
		if(_spawnedEntities.Count > 0)
		{
			Destroy(_spawnedEntities.LastOrDefault());
			_spawnedEntities.RemoveAt(_spawnedEntities.Count - 1);
		}
	}

	private void ActivateComponent()
	{
		if (_spawnedEntities.Count <= 0)
		{
			Debug.LogError("No entities present!");
			return;
		}

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag(TagToToggle))
		{
			foreach (MeshRenderer comp in obj.GetComponentsInChildren<MeshRenderer>())
			{
				MeshRenderer _rend = comp.GetComponent<MeshRenderer>();
				if (_rend)
				{
					_rend.enabled = true;
				}
			}
			MeshRenderer rend = obj.GetComponent<MeshRenderer>();
			if (rend)
			{
				rend.enabled = true;
			}
		}
	}

	private void DeactivateComponent()
	{
		if (_spawnedEntities.Count <= 0)
		{
			Debug.LogError("No entities present!");
			return;
		}

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag(TagToToggle))
		{
			foreach (MeshRenderer comp in obj.GetComponentsInChildren<MeshRenderer>())
			{
				MeshRenderer _rend = comp.GetComponent<MeshRenderer>();
				if (_rend)
				{
					_rend.enabled = false;
				}
			}
			MeshRenderer rend = obj.GetComponent<MeshRenderer>();
			if (rend)
			{
				rend.enabled = false;
			}
		}
	}
}