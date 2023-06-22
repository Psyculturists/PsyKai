using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Tools/SkillBuilder", fileName = "SkillBuilder")]
public class SkillBuilderSO : ScriptableObject
{
    public string skillName = "New Skill";
    public string description;

    public int damage;
    public float attackScaling = 100;

    public int levelRequired = 1;

    public bool isHeal;
    public bool isSelfTarget;

    public List<Skill> createdSkills = new List<Skill>();

    public Skill selectedSkill;
}
