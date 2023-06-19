using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleLogMessage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI logText;

    public void Initialise(string text)
    {
        logText.text = text;
    }
}
