using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StatusEffectHover : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hoverText;

    public void OpenForStatus(StatusEffect effect)
    {
        string textString = "";

        int indexer = 0;
        foreach(StatEffectData data in effect.StatsImpacted)
        {
            textString += System.Enum.GetName(typeof(StatType), data.affectedStat) + (data.isBuff ? " <color=green>UP</color> " : " <color=red>DOWN</color> ") + data.changeValue + (data.changeType == StatChangeType.Flat ? "" : "%");
            indexer++;
            if(indexer < effect.StatsImpacted.Count)
            {
                textString += '\n';
            }
        }

        hoverText.text = textString;
    }
}
