using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            Time.timeScale = 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = 2.0f;
        }
    }
}
