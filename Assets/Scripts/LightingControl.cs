using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * Controls the lighting transitions between ground and underground
 */
public class LightingControl : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D undergroundLight;
    public UnityEngine.Rendering.Universal.Light2D surfaceLight;
    public UnityEngine.Rendering.Universal.Light2D playerLight;
    public UnityEngine.Rendering.Universal.Light2D bgLight;
    Collider2D coll;

    bool isOnSurface;
    [SerializeField] float targetSurfaceIntensity;
    [SerializeField] float targetUndergroundIntensity;
    [SerializeField] float targetPlayerIntensity;

    bool enteringUnderground; //prevents data races between entering and exiting underground


    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<PolygonCollider2D>();
        undergroundLight.intensity = 0;
        playerLight.intensity = 0;
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
            StartCoroutine(EnterUnderground());
        }
	}

	private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            StartCoroutine(ExitUnderground());

        }
    }

	IEnumerator EnterUnderground() {
        enteringUnderground = true;
        undergroundLight.enabled = true;
        playerLight.enabled = true;
        while (enteringUnderground && surfaceLight.intensity > 0) {
            undergroundLight.intensity += targetUndergroundIntensity / 10;
            surfaceLight.intensity -= targetSurfaceIntensity / 10;
            playerLight.intensity += targetPlayerIntensity / 10;
            bgLight.intensity -= 0.1f;
            yield return new WaitForSecondsRealtime(0.05f);
		}
        if (enteringUnderground) {//only disable if the process wasn't cut short
            surfaceLight.enabled = false;
            bgLight.enabled = false;
            yield return null;
        }

    }
    IEnumerator ExitUnderground() {
        //this is timed to finish in 0.5 seconds
        enteringUnderground = false;
        surfaceLight.enabled = true;
        bgLight.enabled = true;
        while (!enteringUnderground && undergroundLight.intensity > 0) {
            undergroundLight.intensity -= targetUndergroundIntensity/10;
            surfaceLight.intensity += targetSurfaceIntensity/10;
            playerLight.intensity -= targetPlayerIntensity / 10;
            bgLight.intensity += 0.1f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        if(!enteringUnderground) { //only disable if the process wasn't cut short
            undergroundLight.enabled = false;
            playerLight.enabled = false;
            yield return null;
        }

    }
}
