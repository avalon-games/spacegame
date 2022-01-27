using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool pause;
    [SerializeField]
    private GameObject settingsMenu;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        settingsMenu.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void OnReturnSelected()
    {
        Resume();
    }

    private void Resume()
    {
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
    }

    private void Pause()
    {
        settingsMenu.SetActive(true);
        Time.timeScale = 0f;
        pause = true;
    }
}
