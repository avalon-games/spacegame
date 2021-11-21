using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour
{
    [SerializeField] private static int numberOfHearts = 8;
    public Image[] hearts;
    private Sprite fullHeart;

    public void Start()
    {
        Debug.Log(numberOfHearts);
    }

    public void setMaxHearts() {
        setHearts(hearts.Length);
    }

    public void removeHeart() {
        setHearts(numberOfHearts - 1);
    }

    public void addHeart() {
        setHearts(numberOfHearts + 1);
    }

    private void setHearts(int num) {
        numberOfHearts = num;
        for (int i = 0; i < hearts.Length; i++) {
            if (i < numberOfHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }    
    }

    public int getHealth()
    {
        return numberOfHearts;
    }

}
