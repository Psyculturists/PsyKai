using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StatData
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int attack;
    [SerializeField]
    private int defence;
    [SerializeField]
    private int speed;

    public int Health => health;
    public int Attack => attack;
    public int Defence => defence;
    public int Speed => speed;

    public void ModifyStat(StatType stat, StatChangeType changeType, float change, int overrideValue = -1)
    {
        switch(stat)
        {
            case StatType.Attack:
                attack = ModifiedAmount(overrideValue == -1 ? Attack : overrideValue, changeType, change);
                break;
            case StatType.Defence:
                defence = ModifiedAmount(overrideValue == -1 ? Defence : overrideValue, changeType, change);
                break;
            case StatType.Speed:
                speed = ModifiedAmount(overrideValue == -1 ? Speed : overrideValue, changeType, change);
                break;
            case StatType.Health:
                health = ModifiedAmount(overrideValue == -1 ? Health : overrideValue, changeType, change);
                break;
        }
    }

    private int ModifiedAmount(int originalValue, StatChangeType changeType, float change)
    {
        int returnVal = originalValue;
        switch(changeType)
        {
            case StatChangeType.Flat:
                returnVal = Mathf.Clamp(originalValue + Mathf.RoundToInt(change), 0, int.MaxValue);
                break;
            case StatChangeType.Percent:
                returnVal = Mathf.RoundToInt(Mathf.Clamp((float)originalValue * ((100.0f + change) / 100.0f), 0, int.MaxValue));
                break;
        }
        return returnVal;
    }

    public void AddStats(StatData otherData)
    {
        attack += otherData.Attack;
        defence += otherData.Defence;
        speed += otherData.Speed;
        health += otherData.Health;
    }
}

public enum StatType
{
    Attack,
    Defence,
    Speed,
    Health,
}
