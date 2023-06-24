using UnityEngine;
using System;

public class ChoiceButtonController : MonoBehaviour
{
    public ChoiceControllerSO bridgeController;
    
    public void OnClick()
    {
        var value = transform.GetSiblingIndex();
        bridgeController.OnChoiceClick(value);
    }
}