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
    Slider oxygenUI;
    [Range(0,10)] public int oxygenDepletionRate; //how much the oxygen depletes each second

    public Slider volume;

    void Start()
    {
        healthUI = this.gameObject.FindComponentsInChildrenWithTag<Image>("Heart"); //make sure to tag the heart images
        oxygenUI = this.gameObject.FindComponentInChildWithTag<Slider>("Oxygen"); //make sure to tag the oxygen UI

        volume.value = PlayerData.volume;
        UpdateHealth();
        UpdateOxygen();

        StartCoroutine(DecreaseOxygen());
        //Debug.Log(healthUI.Length);
    }
	private void Update() {
        UpdateOxygen();
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

    /**
     * Depletes the oxygen by a certain amount every second
     */
    IEnumerator DecreaseOxygen() {
        while (PlayerData.currOxygen > 0) {
            yield return new WaitForSeconds(1);
            PlayerData.currOxygen -= oxygenDepletionRate;
        }
        SceneChanger.GoToSpaceship();

    }

}