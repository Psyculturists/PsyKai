using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeBenefitUI : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TextMeshProUGUI benefitText;

    public void SetForStat(StatType stat, int value)
    {
        benefitText.text = "";
        //iconImage.sprite = ingredient.Icon;
        if(value > 0)
        {
            benefitText.text = "+";
        }
        benefitText.text += value + " " + stat.ToString();
    }

    public void SetForStatus(StatusEffect effect, StatEffectData data, int duration)
    {
        iconImage.sprite = effect.Icon;
        benefitText.text = "+" + data.changeValue.ToString();
        if(data.changeType == StatChangeType.Percent)
        {
            benefitText.text += "%";
        }
        benefitText.text += " " + data.affectedStat.ToString() + " (2 turns)";
    }
}
