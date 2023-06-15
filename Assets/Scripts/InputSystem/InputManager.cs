using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public InputSetup InputScheme;

    public static System.Action OnCreated;

    private Dictionary<Inputs, Dictionary<int, List<InputButtonIndicator>>> SortedInputs = new Dictionary<Inputs, Dictionary<int, List<InputButtonIndicator>>>();

    public KeyCode GetAssignedCode(Inputs input)
    {
        return InputScheme.GameInputs[input];
    }

    public string CodeToString(KeyCode code)
    {
        if(code >= KeyCode.A && code <= KeyCode.Z)
        {
            return System.Convert.ToChar((int)code - 32).ToString();
        }
        else if(code >= KeyCode.Alpha0 && code <= KeyCode.Alpha9)
        {
            return ((int)(code - 48)).ToString();
        }
        return code.ToString();
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            InitialiseInputDictionary();
            OnCreated?.Invoke();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void InitialiseInputDictionary()
    {
        for(int i = 0; i < System.Enum.GetNames(typeof(Inputs)).Length; i++)
        {
            SortedInputs.Add((Inputs)i, new Dictionary<int, List<InputButtonIndicator>>());
        }
    }

    public void RegisterInputButton(InputButtonIndicator button)
    {
        if(!SortedInputs[button.RequiredInput].ContainsKey(button.Priority))
        {
            SortedInputs[button.RequiredInput][button.Priority] = new List<InputButtonIndicator>();
        }
        SortedInputs[button.RequiredInput][button.Priority].Add(button);
    }

    public void DeregisterInputButton(InputButtonIndicator button)
    {
        SortedInputs[button.RequiredInput][button.Priority].Remove(button);
    }

    //this needs to do something with priorities...
    public bool InputButtonHasPriority(InputButtonIndicator button)
    {
        foreach(var v in SortedInputs[button.RequiredInput])
        {
            if(v.Key > button.Priority && v.Value.Count > 0)
            {
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

public enum Inputs
{
    Button1,
    Button2,
    Button3,
    Button4,
    Button5,
    Button6,
    StickNW,
    StickNN,
    StickNE,
    StickEE,
    StickSE,
    StickSS,
    StickSW,
    StickWW,
}

public struct InputPair
{
    public Inputs input;
    public KeyCode inputKey;
}