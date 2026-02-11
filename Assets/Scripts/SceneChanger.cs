using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class SceneChanger
{
	// Timings
	private const float FadeOutTime = 0.35f;
	private const float BlackHoldTime = 1.0f;
	private const float FadeInTime = 0.35f;

	private static bool isTransitioning;

	private static SceneTransitionRunner runner;
	private static Image fadeImage;

	public static void ChangeScene(string sceneName)
	{
		EnsureTransitionObjects();

		if (isTransitioning)
			return;

		string path = $"Scenes/{sceneName}";
		runner.StartCoroutine(Transition(path));
	}

	public static void SceneChangeOnFKey()
	{
		Dictionary<KeyCode, string> functionKeys = new Dictionary<KeyCode, string>
		{
			{ KeyCode.F1,  "Lab01"},
			{ KeyCode.F2,  "Lab02"},
			{ KeyCode.F3,  "Lab03"},
			{ KeyCode.F4,  "Lab04"},
			{ KeyCode.F5,  "Lab05"},
			{ KeyCode.F6,  "Lab06"},
			{ KeyCode.F7,  "Lab07"},
			{ KeyCode.F8,  "Lab08"},
			{ KeyCode.F9,  "Lab09"},
			{ KeyCode.F10, "Lab10"},
			{ KeyCode.F11, "Lab11"},
		};

		foreach (KeyCode key in functionKeys.Keys)
		{
			if (Input.GetKeyDown(key))
				ChangeScene(functionKeys[key]);
		}
	}

	private static void EnsureTransitionObjects()
	{
		if (runner != null && fadeImage != null)
			return;

		GameObject root = new GameObject("SceneChangerTransition (DontDestroyOnLoad)");
		Object.DontDestroyOnLoad(root);

		runner = root.AddComponent<SceneTransitionRunner>();

		// Canvas
		GameObject canvasGO = new GameObject("FadeCanvas");
		canvasGO.transform.SetParent(root.transform, false);

		Canvas canvas = canvasGO.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = short.MaxValue;

		CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

		canvasGO.AddComponent<GraphicRaycaster>();

		// Fullscreen black image
		GameObject imgGO = new GameObject("FadeImage");
		imgGO.transform.SetParent(canvasGO.transform, false);

		fadeImage = imgGO.AddComponent<Image>();
		fadeImage.color = new Color(0f, 0f, 0f, 0f);
		fadeImage.raycastTarget = true;

		RectTransform rt = fadeImage.rectTransform;
		rt.anchorMin = Vector2.zero;
		rt.anchorMax = Vector2.one;
		rt.offsetMin = Vector2.zero;
		rt.offsetMax = Vector2.zero;
	}

	private static IEnumerator Transition(string scenePathOrName)
	{
		isTransitioning = true;

		yield return FadeTo(1f, FadeOutTime);

		SceneManager.LoadScene(scenePathOrName);

		// Let the new scene render at least one frame
		yield return null;

		if (BlackHoldTime > 0f)
			yield return new WaitForSecondsRealtime(BlackHoldTime);

		yield return FadeTo(0f, FadeInTime);

		isTransitioning = false;
	}

	private static IEnumerator FadeTo(float targetAlpha, float duration)
	{
		float startAlpha = fadeImage.color.a;
		float t = 0f;

		while (t < duration)
		{
			t += Time.unscaledDeltaTime;
			float a = Mathf.Lerp(startAlpha, targetAlpha, duration <= 0f ? 1f : (t / duration));
			fadeImage.color = new Color(0f, 0f, 0f, a);
			yield return null;
		}

		fadeImage.color = new Color(0f, 0f, 0f, targetAlpha);
	}

	// Hidden runner so the static class can run coroutines
	private class SceneTransitionRunner : MonoBehaviour { }
}
