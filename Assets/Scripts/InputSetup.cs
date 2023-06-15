using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Data", menuName = "Input Data")]
public class InputSetup : ScriptableObject
{
    [SerializeField]
    private InputDictionary gameInputs = new InputDictionary()
    {
        {Inputs.Button1, KeyCode.Alpha1 },
        {Inputs.Button2, KeyCode.Alpha2 },
        {Inputs.Button3, KeyCode.Alpha3 },
        {Inputs.Button4, KeyCode.Alpha4 },
        {Inputs.Button5, KeyCode.Alpha5 },
        {Inputs.Button6, KeyCode.Alpha6 },
        {Inputs.StickNW, KeyCode.Q },
        {Inputs.StickNN, KeyCode.W },
        {Inputs.StickNE, KeyCode.E },
        {Inputs.StickEE, KeyCode.D },
        {Inputs.StickSE, KeyCode.C },
        {Inputs.StickSS, KeyCode.X },
        {Inputs.StickSW, KeyCode.Z },
        {Inputs.StickWW, KeyCode.A },
    };
    public InputDictionary GameInputs => gameInputs;
}
