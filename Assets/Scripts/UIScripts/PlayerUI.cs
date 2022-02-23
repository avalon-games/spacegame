using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class acts as the middleman between the onscreen UI and the player data
 * It updates the Onscreen UI based on current values in playerdata
 * - manages all player ui elements - hp,oxygen,map,objectives,journal etc.
 * This script should be attached to the UI canvas
 * - it should implement functions that control what is displayed on the UI
 * - to setup, health images and oxygen slider need to be tagged "Heart" and "Oxygen"
 * - also manages oxygen depletion and instantiates volume preferences on scene switch
 * - this script should only be instantiated where the player is present
 */
public class PlayerUI : MonoBehaviour
{

	public Slider volume;
	[SerializeField] private Slider slowmoSlider;
	[SerializeField] GameObject screenTint;

	public float timeToNoSlowmo = 1f;
	//public float timeRemaining;

	#region OXYGEN
	
	Image[] healthUI;
	
	// [Range(0,10)] public int oxygenDepletionRate; //how much the oxygen depletes each second

	private bool slowmoOnCoroutineActive;
	private bool slowmoOffCoroutineActive;
	private bool tintOnCoroutineActive;
	private bool tintOffCoroutineActive;
    #endregion

    void Start()
	{
		healthUI = this.gameObject.FindComponentsInChildrenWithTag<Image>("Heart"); //make sure to tag the heart images
		// oxygenUI = this.gameObject.FindComponentInChildWithTag<Slider>("Oxygen"); //make sure to tag the oxygen UI
		slowmoOnCoroutineActive = false;
		slowmoOffCoroutineActive = false;
		//timeRemaining = timeToNoOxygen;

		volume.value = PlayerData.volume;
		// UpdateHealth();
		// UpdateOxygen();

		// StartCoroutine(DecreaseOxygen());
		//Debug.Log(healthUI.Length);
	}

	private void Update() {
		// UpdateOxygen();
	}

	/**
	 * Updates UI hearts to match the current health in playerdata
	 */
	public void UpdateHealth() {
		for(int i =0; i < healthUI.Length; i++) {
			
			if (i < PlayerData.currHealth)
				healthUI[i].enabled = true;
			else
				healthUI[i].enabled = false;
		}
	}

	/**
	 * Updates Oxygen Level to match the current oxygen level
	 */
	public void UpdateOxygen() {
		slowmoSlider.maxValue = PlayerData.maxOxygen;
		slowmoSlider.value = PlayerData.currOxygen;
	}

	/*
	 * 
	 *  OXYGEN CODE NEEDS REFACTORING
	 * 
	*/

	// requires trigger
	public void ActivateSlowmo()
    {
		Time.timeScale = 0.4f;
		ToggleScreenTint(true);
		if (slowmoOffCoroutineActive)
        {
			StopCoroutine("IncreaseSlowmoMeter");
			slowmoOffCoroutineActive = false;
        }

		if (!slowmoOnCoroutineActive)
        {
			float totalTime = (slowmoSlider.value / slowmoSlider.maxValue) * timeToNoSlowmo;
			StartCoroutine("DecreaseSlowmoMeter", totalTime);
        }
    }

	// requires trigger
	public void DeactivateSlowmo()
    {
		Time.timeScale = 1f;
		ToggleScreenTint(false);
		if (slowmoOnCoroutineActive)
		{
			StopCoroutine("DecreaseSlowmoMeter");
			slowmoOnCoroutineActive = false;
		}

		if (!slowmoOffCoroutineActive)
		{
			float totalTime = timeToNoSlowmo - ((slowmoSlider.value / slowmoSlider.maxValue) * timeToNoSlowmo);
			StartCoroutine("IncreaseSlowmoMeter", totalTime*2);
		}
	}

	/**
	 * Depletes the oxygen by a certain amount every second
	 */
	IEnumerator DecreaseSlowmoMeter(float totalTime) 
	{
		slowmoOnCoroutineActive = true;

		float startTime = Time.time;
		float startValue = slowmoSlider.value;
		float endValue = 0;

		while (Time.time < startTime + totalTime)
		{
			slowmoSlider.value = Mathf.Lerp(startValue, endValue, (Time.time - startTime) / totalTime);
			yield return null;
		}

		slowmoOnCoroutineActive = false;
		slowmoSlider.value = endValue;
		DeactivateSlowmo();
	}

	IEnumerator IncreaseSlowmoMeter(float totalTime)
	{
		slowmoOffCoroutineActive = true;

		float startTime = Time.time;
		float startValue = slowmoSlider.value;
		float endValue = 1;

		while (Time.time < startTime + totalTime)
		{
			slowmoSlider.value = Mathf.Lerp(startValue, endValue, (Time.time - startTime) / totalTime);
			yield return null;
		}

		slowmoOffCoroutineActive = false;
		slowmoSlider.value = endValue;
	}

	public float GetSlowmoCharge() {
		return slowmoSlider.value;
	}
	void ToggleScreenTint(bool toggle) {
		if (toggle) {
			if (tintOffCoroutineActive) {
				StopCoroutine("TintTransitionOff");
				tintOffCoroutineActive = false;
			}
			if (!tintOnCoroutineActive) {
				StartCoroutine("TintTransitionOn", 0.1f);
			}
		} else {
			if (tintOnCoroutineActive) {
				StopCoroutine("TintTransitionOn");
				tintOnCoroutineActive = false;
			}
			if (!tintOffCoroutineActive) {
				StartCoroutine("TintTransitionOff", 0.1f);
			}
		}
		
	}

	IEnumerator TintTransitionOn(float totalTime) {
		tintOnCoroutineActive = true;
		Image img = screenTint.GetComponent<Image>();
		float startValue = img.color.a;
		float endValue = 0.3f;
		float startTime = Time.time;

		while (Time.time < startTime + totalTime) {
			img.color = new Color(0,0,0,Mathf.Lerp(startValue, endValue, (Time.time - startTime) / totalTime));
			yield return null;
		}

		tintOnCoroutineActive = false;
		img.color = new Color(0, 0, 0, endValue);
	}

	IEnumerator TintTransitionOff(float totalTime) {
		tintOffCoroutineActive = true;

		Image img = screenTint.GetComponent<Image>();
		float startValue = img.color.a;
		float endValue = 0;
		float startTime = Time.time;

		while (Time.time < startTime + totalTime) {
			img.color = new Color(0, 0, 0, Mathf.Lerp(startValue, endValue, (Time.time - startTime) / totalTime));
			yield return null;
		}

		tintOffCoroutineActive = false;
		img.color = new Color(0, 0, 0, endValue);
	}
}