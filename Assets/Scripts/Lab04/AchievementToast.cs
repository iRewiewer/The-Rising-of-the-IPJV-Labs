using UnityEngine;
using TMPro;
using System.Collections;

public class AchievementToast : MonoBehaviour
{
	[SerializeField] TMP_Text label;
	[SerializeField] float popScale = 1.0f;
	[SerializeField] float fadeIn = 0.15f;
	[SerializeField] float hold = 1.2f;
	[SerializeField] float fadeOut = 0.3f;

	CanvasGroup cg;
	RectTransform rt;
	bool isPlaying;

	void Awake()
	{
		cg = GetComponent<CanvasGroup>();
		rt = GetComponent<RectTransform>();
		cg.alpha = 0f;
		rt.localScale = Vector3.one;
		gameObject.SetActive(false);
	}

	public void Show(string message)
	{
		if (isPlaying) return;
		label.text = message;
		gameObject.SetActive(true);
		transform.SetAsLastSibling();
		StartCoroutine(Play());
	}

	IEnumerator Play()
	{
		isPlaying = true;

		// pop-in + fade-in
		cg.alpha = 0f;
		rt.localScale = Vector3.one * popScale / 2;
		float t = 0f;
		while (t < fadeIn)
		{
			t += Time.unscaledDeltaTime;
			float k = t / fadeIn;
			cg.alpha = k;
			rt.localScale = Vector3.Lerp(Vector3.one * popScale, Vector3.one, k);
			yield return null;
		}
		cg.alpha = 1f;
		rt.localScale = Vector3.one;

		// hold
		t = 0f;
		while (t < hold)
		{
			t += Time.unscaledDeltaTime;
			yield return null;
		}

		// fade-out
		t = 0f;
		while (t < fadeOut)
		{
			t += Time.unscaledDeltaTime;
			float k = t / fadeOut;
			cg.alpha = 1f - k;
			yield return null;
		}
		cg.alpha = 0f;

		gameObject.SetActive(false);
		isPlaying = false;
	}
}
