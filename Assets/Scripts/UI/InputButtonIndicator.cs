using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputButtonIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonLetterText;
    [SerializeField]
    private Button button;
    [SerializeField]
    private Inputs requiredInput;
    public Inputs RequiredInput => requiredInput;
    [SerializeField]
    private int priority = 0;
    public int Priority => priority;

    private KeyCode cachedKeycode = KeyCode.S;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            Init();
        }
        else
        {
            InputManager.OnCreated += AwaitInputManager;
        }
    }

    private void AwaitInputManager()
    {
        InputManager.OnCreated -= AwaitInputManager;
        Init();
    }

    private void Init()
    {
        cachedKeycode = InputManager.Instance.GetAssignedCode(requiredInput);
        buttonLetterText.text = InputManager.Instance.CodeToString(cachedKeycode);

        InputManager.Instance.RegisterInputButton(this);
    }

    private void OnDisable()
    {
        InputManager.Instance.DeregisterInputButton(this);
    }

    private void RequestToExecute()
    {
        if(InputManager.Instance.InputButtonHasPriority(this))
        {
            button.onClick?.Invoke();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(cachedKeycode))
        {
            if (button.interactable)
            {
                RequestToExecute();
            }
        }
    }
}
