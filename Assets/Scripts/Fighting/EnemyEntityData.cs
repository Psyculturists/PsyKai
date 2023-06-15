using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Data/Enemy Data")]
public class EnemyEntityData : CombatEntityData
{
    [SerializeField]
    private string entityName;
    public string EntityName => entityName;
    [SerializeField]
    protected Enemy entityPrefab;
    public Enemy EntityPrefab => entityPrefab;
}
