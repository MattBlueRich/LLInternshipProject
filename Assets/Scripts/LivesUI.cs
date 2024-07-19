using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    private int lastLives;
    private void Start()
    {
        lastLives = PlayerLives.maxLives;
        UpdateLivesUI(lastLives);
    }

    private void Update()
    {
        if(PlayerLives.maxLives != lastLives) 
        {
            lastLives = PlayerLives.maxLives;
            UpdateLivesUI(lastLives);
        }
    }
    public void UpdateLivesUI(int currentLives)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(i < currentLives)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
