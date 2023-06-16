using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Data/Player Data")]
public class PlayerEntityData : CombatEntityData
{
    [SerializeField]
    protected PlayerEntity entityPrefab;
    public PlayerEntity EntityPrefab => entityPrefab;

    [SerializeField]
    private StatData finalStats;
    public StatData FinalStats => finalStats;

    public StatData StatsForCurrentLevel => StatData.GetAveragedStatsForLevelBetweenTwoProfiles(BaseStats, FinalStats, PlayerDataManager.Instance.PlayerLevel, PlayerDataManager.Instance.MaxLevel);
}
