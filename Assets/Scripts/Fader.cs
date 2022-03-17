using UnityEngine;
using System.Collections;
public class Fader : MonoBehaviour
{
	CanvasGroup canvasGroup;
	bool isFadingIn;
	bool isFadingOut;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void FadeOutImmediate()
	{
		canvasGroup.alpha = 1;
	}

	public IEnumerator FadeOut(float time)
	{
		canvasGroup.blocksRaycasts = true;
		while (isFadingIn)
		{
			yield return null;
		}
		isFadingOut = true;
		while (canvasGroup.alpha < 1)
		{
			canvasGroup.alpha += Time.deltaTime / time;
			yield return null;
		}

		canvasGroup.alpha = 1;
		isFadingOut = false;
		canvasGroup.blocksRaycasts = false;
	}

	public IEnumerator FadeIn(float time)
	{
		canvasGroup.blocksRaycasts = true;
		while (isFadingOut)
		{
			yield return null;
		}
		isFadingIn = true;
		while (canvasGroup.alpha > 0)
		{
			canvasGroup.alpha -= Time.deltaTime / time;
			yield return null;
		}
		canvasGroup.alpha = 0;
		isFadingIn = false;
		canvasGroup.blocksRaycasts = false;
	}

	public IEnumerator FadeOutIn()
	{
		yield return FadeOut(2f);
		yield return FadeIn(1f);
	}
}