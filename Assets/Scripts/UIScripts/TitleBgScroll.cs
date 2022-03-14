using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBgScroll : MonoBehaviour
{
    [SerializeField] float scrollTime = 10f;
    [SerializeField] float yStart = 0f;
    [SerializeField] float yEnd = 696.7002f;
    float timer = 0;
    //[SerializeField] float y = 0f;

    RectTransform rect;
    // Start is called before the first frame update
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
    }

	private void Update() {
        float fraction = timer / scrollTime;

		if (fraction <= 1) {
			float yPos = Mathf.Lerp(yStart, yEnd, fraction);
			rect.anchoredPosition = new Vector2(0f, yPos);
			timer += Time.deltaTime;
		}
		//rect.anchoredPosition = new Vector2(0f, y);

	}
}
