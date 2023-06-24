using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingHealthChange : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI numberText;
    [SerializeField]
    private Color healColour;
    [SerializeField]
    private Color harmColour;
    [SerializeField]
    private Color nullColour;
    [SerializeField]
    private CanvasGroup group;

    [SerializeField]
    private float lifetime = 1.5f;
    [SerializeField]
    private float driftDistance = 100.0f;
    [SerializeField]
    private float fadeOutTime = 0.5f;

    private float remainingLifetime = 0.0f;
    private float remainingFadeTime = 0.0f;

    public void Initialise(int number)
    {
        remainingLifetime = lifetime;
        numberText.color = number > 0 ? healColour : number < 0 ? harmColour : nullColour;
        numberText.text = (number > 0 ? "+" : "") + number.ToString();
        remainingFadeTime = fadeOutTime;
    }

    public void Initialise(StatEffectData effect)
    {
        numberText.text = "+ Effect";
        numberText.color = effect.isBuff ? healColour : harmColour;
        remainingLifetime = lifetime;
        remainingFadeTime = fadeOutTime;
    }

    private void FadeOut()
    {
        group.alpha = remainingFadeTime / fadeOutTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += transform.up * (driftDistance / lifetime) * Time.deltaTime;
        remainingLifetime -= Time.deltaTime;

        if(remainingLifetime <= lifetime - fadeOutTime)
        {
            FadeOut();
            fadeOutTime -= Time.deltaTime;
        }
        else if(remainingLifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
