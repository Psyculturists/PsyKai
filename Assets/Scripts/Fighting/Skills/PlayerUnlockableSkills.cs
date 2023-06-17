using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unlockable Skill Container", menuName = "Data/Unlockable Skill List")]
public class PlayerUnlockableSkills : ScriptableObject
{
    [SerializeField]
    private List<Skill> playerUnlockables = new List<Skill>();
    public List<Skill> PlayerUnlockables => playerUnlockables;

}
