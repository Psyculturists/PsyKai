using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StatusEffectBar : MonoBehaviour
{
    [SerializeField]
    private StatusIndicator statusPrefab;
    [SerializeField]
    private Transform statusParent;
    [SerializeField]
    private StatusEffectHover hoverPrefab;
    [SerializeField]
    private Transform hoverParent;

    private List<StatusIndicator> spawnedStatusIndicators = new List<StatusIndicator>();


    public void AddStatusEffect(StatusEffect status, int turns)
    {
        StatusIndicator prevIndicator = spawnedStatusIndicators.Find(i => i.IsSetToStatusEffect(status));
        if (prevIndicator != null)
        {
            prevIndicator.UpdateStatus(status, turns);
            return;
        }

        StatusIndicator indicator = Instantiate(statusPrefab, statusParent);
        indicator.UpdateStatus(status, turns);
        indicator.SetOnHover(SpawnHover, ClearHover);
        spawnedStatusIndicators.Add(indicator);
    }

    private void SpawnHover(StatusEffect effect)
    {
        StatusEffectHover hover = Instantiate(hoverPrefab, hoverParent);
        hover.OpenForStatus(effect);
    }

    private void ClearHover()
    {
        hoverParent.DestroyAllChildren();
    }
    

    public void UpdateAllStatusOnTurn()
    {
        for(int i = spawnedStatusIndicators.Count - 1; i >= 0; i--)
        {
            spawnedStatusIndicators[i].OnTurnPassed();
        }
    }
}
