using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private float primaryXPExponent = 3f;
    [SerializeField]
    private float primaryXPMultiplier = 2f;
    [SerializeField]
    private float primaryXPFlatAmount = 10f;
    
    [SerializeField]
    public string levelXPFormula => $"Level Exp Formula = level^{primaryXPExponent} * {primaryXPMultiplier} + {primaryXPFlatAmount}";

    
    public static int GetXPRequiredForNextLevel(int currentLevel)
    {
        return Mathf.CeilToInt(Instance.primaryXPMultiplier * Mathf.Pow(currentLevel, Instance.primaryXPExponent) + Instance.primaryXPFlatAmount);
    }

    public static (int LevelsGained, int ExpRemaining) GetLevelsGainedAndExpRemainder(int startingLevel, int gainedExp)
    {
        return _GetLevelsGainedAndExpRemainder(startingLevel, gainedExp, x => GetXPRequiredForNextLevel(x));
    }

    private static (int LevelsGained, int ExpRemaining) _GetLevelsGainedAndExpRemainder(int startingLevel, int gainedExp, Func<int, int> GetXPForNextLevel)
    {
        int levels = 0;
        int nextLevel = startingLevel;
        int expRemaining = gainedExp;

        while (expRemaining > 0)
        {
            int expReq = GetXPForNextLevel(nextLevel++);
            if (expRemaining >= expReq && expReq != 0)
            {
                levels += 1;
                expRemaining -= expReq;
            }
            else
            {
                break;
            }
        }

        return (levels, expRemaining);
    }

    public static (int Level, int Exp) GetPlayerLevelAndExpAfterSubtracting(int exp, int startLevel, int startExp)
    {
        return _GetLevelAndExpAfterSubtracting(exp, startLevel, startExp,
            x => GetXPRequiredForNextLevel(x), (x, y) => GetLevelsGainedAndExpRemainder(x, y));
    }

    private static (int Level, int Exp) _GetLevelAndExpAfterSubtracting(int exp, int endLevel, int endExp,
        Func<int, int> GetXPForNextLevel, Func<int, int, (int LevelsGained, int ExpRemaining)> GetLevelsGained)
    {
        if (exp == 0)
        {
            return (endLevel, endExp);
        }

        if (endLevel == 1)
        {
            return (1, Mathf.Max(0, endExp - exp));
        }

        int currentLevel = endLevel - 1;
        int totalExp = exp;
        while (totalExp > 0)
        {
            int expReq = GetXPForNextLevel(currentLevel);
            totalExp -= expReq;
            if (totalExp > 0)
            {
                currentLevel--;
                if (currentLevel <= 0)
                {
                    return (1, 0);
                }
            }
        }
        totalExp = Mathf.Abs(totalExp);

        var (LevelsGained, ExpRemaining) = GetLevelsGained(currentLevel, endExp + totalExp);
        (int Level, int Exp) result = (currentLevel + LevelsGained, ExpRemaining);
        result.Exp = result.Level < 0 ? 0 : Mathf.Max(result.Exp, 0);
        result.Level = Mathf.Max(result.Level, 0);
        return result;
    }
}
