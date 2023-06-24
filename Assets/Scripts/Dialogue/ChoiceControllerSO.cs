using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObjects/ChoiceController")]
public class ChoiceControllerSO : ScriptableObject
{
    public event Action<int> onClickEvent;

    public void OnChoiceClick(int value)
    {
        onClickEvent?.Invoke(value);
    }
}