using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBgScroll : MonoBehaviour
{
    [SerializeField] float scrollTime = 10f;
    [SerializeField] RectTransform canvas;
    float yStart = 0f;
    float yEnd;
    float timer = 0;
    //[SerializeField] float y = 0f;

    RectTransform bg;
    // Start is called before the first frame update
    void Awake()
    {
        bg = GetComponent<RectTransform>();
        bg.anchoredPosition = Vector2.zero;
    }
	private void Start() {
        yEnd = bg.rect.height - canvas.rect.height;
    }

	private void Update() {
        float fraction = timer / scrollTime;

		if (fraction <= 1) {
			float yPos = Mathf.Lerp(yStart, yEnd, fraction);
			bg.anchoredPosition = new Vector2(0f, yPos);
			timer += Time.deltaTime;
		}
		//rect.anchoredPosition = new Vector2(0f, y);

	}
}
