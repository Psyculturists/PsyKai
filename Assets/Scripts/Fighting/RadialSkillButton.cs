﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleSkillButton : MonoBehaviour
{
    [SerializeField]
    private Image skillImage;
    [SerializeField]
    private TextMeshProUGUI skillText;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject weaknessIndicator;
    [SerializeField]
    private GameObject resistanceIndicator;

    [SerializeField]
    private Skill loadedSkill;

    public bool hasSkillAssigned => loadedSkill != null;

    private System.Action<Skill> clickCallback;

    private void Awake()
    {
        if(loadedSkill)
        {
            SetFields();
        }
        button.onClick.AddListener(TryEvoke);
    }

    private void OnDestroy()
    {
        clickCallback = null;
    }

    public void UpdateResistanceIndication(CombatEntity entitySelected)
    {
        resistanceIndicator.SetActive(entitySelected.Resists(loadedSkill));
        weaknessIndicator.SetActive(entitySelected.WeakTo(loadedSkill));
    }

    public void AssignSkill(Skill newSkill)
    {
        loadedSkill = newSkill;
        SetFields();
    }

    public void SetCallback(System.Action<Skill> callback)
    {
        clickCallback = callback;
    }

    private void TryEvoke()
    {
        clickCallback?.Invoke(loadedSkill);
    }

    public void SetInteractive(bool state)
    {
        button.interactable = state;
    }

    private void SetFields()
    {
        if (loadedSkill)
        {
            skillImage.sprite = loadedSkill.Icon;
            skillText.text = loadedSkill.skillName;
        }
        else
        {
            skillImage.sprite = null;
            skillText.text = null;
        }
    }
}
