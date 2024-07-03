using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileFPSFix : MonoBehaviour
{
    // Fixes slow frame-rate issue while running on mobile.
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
