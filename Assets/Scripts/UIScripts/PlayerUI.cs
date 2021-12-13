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
	Image[] healthUI;
	public Slider volume;

	#region OXYGEN
	[SerializeField]
	private Slider oxygenUI;
	// [Range(0,10)] public int oxygenDepletionRate; //how much the oxygen depletes each second
	public float timeToNoOxygen = 10f;
	public float timeRemaining;
	private bool oxygenLossCoroutineActive;
	private bool oxygenGainCoroutineActive;
    #endregion

    void Start()
	{
		healthUI = this.gameObject.FindComponentsInChildrenWithTag<Image>("Heart"); //make sure to tag the heart images
		// oxygenUI = this.gameObject.FindComponentInChildWithTag<Slider>("Oxygen"); //make sure to tag the oxygen UI
		oxygenLossCoroutineActive = false;
		oxygenGainCoroutineActive = false;
		timeRemaining = timeToNoOxygen;

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
		oxygenUI.maxValue = PlayerData.maxOxygen;
		oxygenUI.value = PlayerData.currOxygen;
	}

	/*
	 * 
	 *  OXYGEN CODE NEEDS REFACTORING
	 * 
	*/

	// requires trigger
	public void ActivateOxygenLoss()
    {
		if (oxygenGainCoroutineActive)
        {
			StopCoroutine("IncreaseOxygen");
			oxygenGainCoroutineActive = false;
        }

		if (!oxygenLossCoroutineActive)
        {
			float totalTime = (oxygenUI.value / oxygenUI.maxValue) * timeToNoOxygen;
			StartCoroutine("DecreaseOxygen", totalTime);
        }
    }

	// requires trigger
	public void StopOxygenLoss()
    {
		if (oxygenLossCoroutineActive)
		{
			StopCoroutine("DecreaseOxygen");
			oxygenLossCoroutineActive = false;
		}

		if (!oxygenGainCoroutineActive)
		{
			float totalTime = timeToNoOxygen - ((oxygenUI.value / oxygenUI.maxValue) * timeToNoOxygen);
			StartCoroutine("IncreaseOxygen", totalTime);
		}
	}

	/**
	 * Depletes the oxygen by a certain amount every second
	 */
	IEnumerator DecreaseOxygen(float totalTime) 
	{
		oxygenLossCoroutineActive = true;

		float startTime = Time.time;
		float startValue = oxygenUI.value;
		float endValue = 0;

		while (Time.time < startTime + totalTime)
		{
			oxygenUI.value = Mathf.Lerp(startValue, endValue, (Time.time - startTime) / totalTime);
			yield return null;
		}

		oxygenLossCoroutineActive = false;
		oxygenUI.value = endValue;
		SceneChanger.GoToSpaceship();
	}

	IEnumerator IncreaseOxygen(float totalTime)
	{
		oxygenGainCoroutineActive = true;

		float startTime = Time.time;
		float startValue = oxygenUI.value;
		float endValue = 1;

		while (Time.time < startTime + totalTime)
		{
			oxygenUI.value = Mathf.Lerp(startValue, endValue, (Time.time - startTime) / totalTime);
			yield return null;
		}

		oxygenGainCoroutineActive = false;
		oxygenUI.value = endValue;
	}
}