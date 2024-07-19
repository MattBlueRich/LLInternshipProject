using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;
    public static int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(levels[currentLevel]);
    }
}
