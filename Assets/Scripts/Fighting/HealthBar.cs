using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image healthOverlay;

    [SerializeField]
    private TextMeshProUGUI healthText;

    private float originalWidth = 250.0f;

    // Start is called before the first frame update
    void Start()
    {
        originalWidth = GetComponent<RectTransform>().sizeDelta.x;
    }

    public void UpdateBar(int current, int max)
    {
        healthOverlay.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((float)current / (float)max) * originalWidth);
        healthText.text = current + " / " + max;
    }
}
