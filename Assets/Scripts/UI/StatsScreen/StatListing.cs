using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatListing : MonoBehaviour
{
    [SerializeField]
    private StatType statType;
    [SerializeField]
    private TextMeshProUGUI statNameText;
    [SerializeField]
    private TextMeshProUGUI statValueText;

    public void Initialise()
    {
        StatData stats = PlayerDataManager.Instance.CurrentLevelStats();
        (string, int) relevantStatInfo = stats.GetStatFromStatType(statType);
        statNameText.text = relevantStatInfo.Item1;
        statValueText.text = relevantStatInfo.Item2.ToString();
    }

    private void OnEnable()
    {
        Initialise();
    }
}
