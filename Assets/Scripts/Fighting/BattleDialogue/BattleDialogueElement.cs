using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleDialogueElement : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private TextMeshProUGUI textField;
    [SerializeField]
    private float lifetime = 5.0f;
    [SerializeField]
    private float fadeOutDuration = 0.25f;
    private float remainingFadeOut = 0.0f;

    private float remainingLifetime = 0.0f;


    public BattleDialogueElement(string text)
    {
        Initialise(text);
    }

    public void Initialise(string text)
    {
        remainingLifetime = lifetime;
        textField.text = text;
        remainingFadeOut = fadeOutDuration;
        SetFadeValue();
    }

    private void SetFadeValue()
    {
        canvasGroup.alpha = remainingFadeOut / fadeOutDuration;
    }

    private void Update()
    {
        if (remainingLifetime > 0)
        {
            remainingLifetime -= Time.deltaTime;
        }
        else if (remainingFadeOut > 0)
        {
            remainingFadeOut -= Time.deltaTime;
            SetFadeValue();
        }
        else
        {
            Destroy(this);
        }
    }
}
