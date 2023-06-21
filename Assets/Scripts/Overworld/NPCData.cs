using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "NPC Data")]
public class NPCData : ScriptableObject
{
    public string name;
    public Sprite portrait;
    public EnemyEntityData entityData;
}