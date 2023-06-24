using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StatusIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image statusIconImage;

    [SerializeField]
    private TextMeshProUGUI tempTextForStat;

    [SerializeField]
    private TextMeshProUGUI turnsRemainingText;

    [SerializeField]
    private StatusEffectHover hoverPrefab;
    [SerializeField]
    private Transform hoverParent;

    private StatusEffect effectIndicating;
    private int turnsRemaining = 0;

    private System.Action<StatusEffect> hoverAction;
    private System.Action hoverStopAction;

    
    public void UpdateStatus(StatusEffect effect, int turnsLeft)
    {
        if(turnsLeft == 0)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            effectIndicating = effect;
            turnsRemaining = turnsLeft;
        }
        UpdateFields();
    }

    private void OnDestroy()
    {
        hoverAction = null;
        hoverStopAction = null;
    }

    public void SetOnHover(System.Action<StatusEffect> hoverEffect, System.Action stopHoverEffect)
    {
        hoverAction = hoverEffect;
        hoverStopAction = stopHoverEffect;
    }

    public bool IsSetToStatusEffect(StatusEffect status)
    {
        return status == effectIndicating;
    }

    public void OnTurnPassed()
    {
        UpdateStatus(effectIndicating, --turnsRemaining);
    }

    private void UpdateFields()
    {
        tempTextForStat.text = System.Enum.GetName(typeof(StatType), effectIndicating.StatsImpacted[0].affectedStat).Substring(0, 1);
        turnsRemainingText.text = turnsRemaining.ToString();
    }

    public void OnStartHover()
    {
        hoverAction?.Invoke(effectIndicating);
    }

    public void OnFinishHover()
    {
        hoverStopAction?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnStartHover();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnFinishHover();
    }
}
