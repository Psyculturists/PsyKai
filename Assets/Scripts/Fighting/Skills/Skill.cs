﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Data/Skill")]
public class Skill : ScriptableObject
{
    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;
    public string skillName;
    [TextArea]
    public string description;
    public int damage;
    public bool heals = false;
    [SerializeField]
    private float attackScaling = 50.0f;
    public float AttackScaling => attackScaling;

    [SerializeField]
    private int orderOfSkill = 0;
    public int OrderOfSkill => orderOfSkill;
    public bool isSelfTargeted;
    [SerializeField]
    private bool hasDelayedResolution = false;
    public bool HasDelayedResolution => hasDelayedResolution;

    [SerializeField]
    private StatusEffect statusEffect;
    public StatusEffect Status => statusEffect;
    private bool hasStatusEffect => statusEffect != null;
    public bool HasStatusEffect => hasStatusEffect;
    [SerializeField]
    private StatusEffectApplicationData applicationData;
    public StatusEffectApplicationData ApplicationData => applicationData;

    public System.Action<bool> OnResolution;


    public int TotalDamageAfterScaling(int usersAttack, int targetDefence)
    {
        float attackRatio = AttackScaling / 100.0f;
        float preMitigated = attackRatio * (float)damage;
        if (targetDefence <= 0) targetDefence = 1;
        float percentDamageAmpFromStatDiff = (float)usersAttack / (float)targetDefence;

        int damageDealt = Mathf.RoundToInt(Mathf.Clamp(preMitigated * percentDamageAmpFromStatDiff, 0, Mathf.Infinity));
        if(heals)
        {
            damageDealt = -damageDealt;
        }
        Debug.Log(damageDealt);
        return damageDealt;
    }

    public bool ResolveSkill(bool returnValue)
    {
        Debug.Log("skill has resolved with value: " + returnValue.ToString());
        OnResolution?.Invoke(returnValue);
        OnResolution = null;
        return returnValue;
    }

    public virtual void AlternativeCastEffect(CombatEntity target)
    {

    }
}
