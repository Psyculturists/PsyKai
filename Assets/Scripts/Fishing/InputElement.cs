using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class InputElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputText;

    private Action<InputElement> OnInputGiven;

    private KeyCode cachedKeycode = KeyCode.S;

    public void Initialise(Inputs input, Action<InputElement> action)
    {
        cachedKeycode = InputManager.Instance.GetAssignedCode(input);
        inputText.text = InputManager.Instance.CodeToString(cachedKeycode);
        OnInputGiven = action;
    }

    private void OnDestroy()
    {
        OnInputGiven = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(cachedKeycode))
        {
            OnInputGiven?.Invoke(this);
        }
    }
}
