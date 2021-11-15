using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour
{
    public int numberOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;

    public void setMaxHearts() {
        setHearts(hearts.Length);
    }

    public void removeHeart() {
        setHearts(numberOfHearts - 1);
    }

    public void addHeart() {
        setHearts(numberOfHearts + 1);
    }

    public void setHearts(int num) {
        numberOfHearts = num;
        for (int i = 0; i < hearts.Length; i++) {
            if (i < numberOfHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }    
    }

}
