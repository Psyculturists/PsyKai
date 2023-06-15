using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Data/Player Data")]
public class PlayerEntityData : CombatEntityData
{
    [SerializeField]
    protected PlayerEntity entityPrefab;
    public PlayerEntity EntityPrefab => entityPrefab;
}
