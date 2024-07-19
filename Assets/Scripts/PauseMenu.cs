using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    public static bool autoPause = true;
    private void Start()
    {
        // Open pause menu automatically when the game has started.
        if (autoPause)
        {
            autoPause = false;
            isPaused = true;
            Time.timeScale = 0.0f;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            CloseMenu();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    public void CloseMenu()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        GameObject.FindGameObjectWithTag("Music Manager").GetComponent<AudioLowPassFilter>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        isPaused = true;
        Time.timeScale = 0.0f;
        GameObject.FindGameObjectWithTag("Music Manager").GetComponent<AudioLowPassFilter>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

}
