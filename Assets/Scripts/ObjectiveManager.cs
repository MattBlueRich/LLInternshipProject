using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    bool objectiveMet = false;
    public TextMeshProUGUI clearText;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!objectiveMet)
        {
            if (GameObject.FindGameObjectsWithTag("Ghost").Length == 0)
            {
                objectiveMet = true;
                clearText.GetComponent<Animator>().SetTrigger("Appear");
                Debug.Log("Ghosts Destroyed!");
                Time.timeScale = 0.0f;
            }
        }
    }
}
