using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Group", menuName = "Data/Enemy Group")]
public class EnemyGroup : ScriptableObject
{
    [SerializeField]
    private string groupName;
    public string GroupName => groupName;
    [SerializeField]
    private List<EnemyEntityData> enemies;
    public List<EnemyEntityData> Enemies => enemies;

    [SerializeField]
    private int suggestedLevel;
    public int SuggestedLevel => suggestedLevel;
}
