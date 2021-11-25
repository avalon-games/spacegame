using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class acts as the middleman between the onscreen UI and the player data
 * It updates the Onscreen UI based on current values in playerdata
 * This script should be attached to the UI canvas
 * - it should implement functions that control what is displayed on the UI
 * - to setup, health images and oxygen slider need to be tagged "Heart" and "Oxygen"
 */
public class PlayerUI : MonoBehaviour
{
    Image[] healthUI;
    Slider oxygenUI;

    void Start()
    {
        healthUI = this.gameObject.FindComponentsInChildrenWithTag<Image>("Heart"); //make sure to tag the heart images
        oxygenUI = this.gameObject.FindComponentInChildWithTag<Slider>("Oxygen"); //make sure to tag the oxygen UI

        UpdateHealth();
        UpdateOxygen();
        //Debug.Log(healthUI.Length);
    }
	private void Update() {
        ////DEBUGGING
        //if (Input.GetKeyDown(KeyCode.G)) {
        //    PlayerData.currHealth -= 1;
        //    UpdateHealth();
        //}
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


}
//add reset playerdata,access playerdata and rendering it on UI
//commands the UI elements to change, acts as the middle man between PlayerData and onscreen UI elements
//this script should be put in canvas so it can access all children UI elements
//add functions for interactible ui - map, healthbar, pause menu,journal,objectives,optionsmenu, titlepage,save slot