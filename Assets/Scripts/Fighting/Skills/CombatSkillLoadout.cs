using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loadout", menuName = "Skill Loadout")]
public class CombatSkillLoadout : ScriptableObject
{
    [SerializeField]
    private Skill[] skills;
    public Skill[] Skills => skills;

    [SerializeField]
    private bool canRun = true;
}
