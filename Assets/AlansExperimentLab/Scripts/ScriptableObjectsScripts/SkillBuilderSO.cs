using UnityEngine;


[CreateAssetMenu(menuName = "Tools/SkillBuilder", fileName = "SkillBuilder")]
public class SkillBuilderSO : ScriptableObject
{
    public string skillName = "New Skill";
    public string description;

    public string damage;

    public bool isHealer;
}
