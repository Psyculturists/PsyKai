using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "NPC info/NPC Data")]
public class NPCData : ScriptableObject
{
    public string name;
    public Sprite portrait;
    public EnemyEntityData entityData;
}

[System.Serializable]
public class TagData 
{
    public string tag;
    public NPCData characterData;
}