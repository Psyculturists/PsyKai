using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Status Effect", menuName = "Data/Status Effect")]
public class StatusEffect : ScriptableObject
{
    [SerializeField]
    private string effectName;
    public string EffectName => effectName;

    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    private List<StatEffectData> statsImpacted;
    public List<StatEffectData> StatsImpacted => statsImpacted;
}

[Serializable]
public struct StatEffectData
{
    public StatType affectedStat;
    public StatChangeType changeType;
    public bool isBuff;
    public float changeValue;
}

public enum StatChangeType
{
    Flat,
    Percent,
}

[Serializable]
public struct StatusEffectApplicationData
{
    public float chance;
    public int turnDuration;
}
