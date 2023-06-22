using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleEntityDialogue : MonoBehaviour
{
    [SerializeField]
    private BattleDialogueElement elementPrefab;
    [SerializeField]
    private Transform elementParent;

    public void SpawnDialogue(string text)
    {
        elementParent.DestroyAllChildren();
        Instantiate(elementPrefab, elementParent).Initialise(text);
    }
}
