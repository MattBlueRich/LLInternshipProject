using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    bool objectiveMet = false;
    public TextMeshProUGUI clearText;
    public GameObject[] levels;
    public static int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        Instantiate(levels[currentLevel]); // Spawn Level
        Debug.Log("Level: " + currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (!objectiveMet)
        {
            if (GameObject.FindGameObjectsWithTag("Ghost").Length == 0)
            {
                objectiveMet = true;
                
                Debug.Log("Ghosts Destroyed!");
                Time.timeScale = 0.0f;

                ScoreManager.instance.SaveScore(); // This saves the player's current score.

                // This if-statement checks if we're on the last level, to not move to any more stages, otherwise load the next level.
                if (currentLevel+1 > levels.Length-1)
                {
                    Debug.Log("Cleared all levels!");
                    clearText.text = "STAGE CLEAR";
                }
                else
                {
                    clearText.text = "CLEAR";
                    currentLevel++;
                    StartCoroutine(LoadNewLevel());
                }

                clearText.GetComponent<Animator>().SetTrigger("Appear");
            }
        }
    }

    IEnumerator LoadNewLevel()
    {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
