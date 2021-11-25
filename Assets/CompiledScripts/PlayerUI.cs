using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class acts as the middleman between the onscreen UI and the player data
 * It updates the Onscreen UI based on current values in playerdata
 * This script should be attached to the UI canvas
 */
public class PlayerUI : MonoBehaviour
{
    SpriteRenderer[] healthUI;

    void Start()
    {
        healthUI = this.gameObject.FindComponentsInChildrenWithTag<SpriteRenderer>("Heart");
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
    public void UpdateOxygen() {

	}


}
//add reset playerdata,access playerdata and rendering it on UI
//commands the UI elements to change, acts as the middle man between PlayerData and onscreen UI elements
//this script should be put in canvas so it can access all children UI elements
//add functions for interactible ui - map, healthbar, pause menu,journal,objectives,optionsmenu, titlepage,save slot