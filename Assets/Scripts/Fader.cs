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
		//UIMenus uiMenu = FindObjectOfType<UIMenus>();
		//if (uiMenu) uiMenu.enabled = false;
		canvasGroup.blocksRaycasts = true;
		while (isFadingIn)
		{
			isFadingOut = false;
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
		//UIMenus uiMenu = FindObjectOfType<UIMenus>();
		//if (uiMenu) uiMenu.enabled = false;
		canvasGroup.blocksRaycasts = true;
		while (isFadingOut)
		{
			isFadingIn = false;
			yield return null;
		}
		isFadingIn = true;
		while (canvasGroup.alpha > 0)
		{
			canvasGroup.alpha -= Time.deltaTime / time;
			yield return null;
		}
		//if (uiMenu) uiMenu.enabled = true;
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