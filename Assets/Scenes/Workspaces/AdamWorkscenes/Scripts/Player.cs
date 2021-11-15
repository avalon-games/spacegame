using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHearts = 8;
    public int currentHearts;
    public HeartBar heartBar;

    public float timeBeforeOxygenLoss = 0.5f;
    public float timer = 0.0f;
    public int maxOxygen = 10;
    public float currentOxygen;
    public OxygenBar oxygenBar;

    // Start is called before the first frame update
    void Start() {
        currentHearts = maxHearts;
        heartBar.setMaxHearts();

        currentOxygen = maxOxygen;
        oxygenBar.setMaxOxygenBar(maxOxygen);
    }

    // Update is called once per frame
    void Update() {
        updateOxygen();
        updateHearts();
    }

    public void updateHearts() {
        if (Input.GetKeyDown(KeyCode.A)) {
            if (currentHearts >= 1) {
                currentHearts = currentHearts - 1;
                heartBar.removeHeart();
            }
        }    
        if (Input.GetKeyDown(KeyCode.R)) {
            if (currentHearts < maxHearts) {
                currentHearts = currentHearts + 1;
                heartBar.addHeart();
            }
        }
            
    }

    public void updateOxygen() {
        timer = timer + Time.deltaTime;
        if (timer >= timeBeforeOxygenLoss) {
            decreaseOxygen(0.05f);
        }
    }

    public void decreaseOxygen(float amount) {
        currentOxygen = currentOxygen - amount;
        oxygenBar.decreaseOxygenBar(amount);
        timer = 0.0f;
    }
}
