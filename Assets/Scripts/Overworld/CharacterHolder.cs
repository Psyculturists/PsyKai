using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC info/Character Holder")]
public class CharacterHolder : ScriptableObject
{
    public Dictionary<string, NPCData> characters;
}