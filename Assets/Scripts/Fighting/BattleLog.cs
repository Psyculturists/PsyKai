using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLog : MonoBehaviour
{
    [SerializeField]
    private BattleLogMessage logPrefab;
    [SerializeField]
    private Transform logParent;

    public void CreateLog(CombatEntity user, CombatEntity target, Skill skill, int damage)
    {
        string output = "";
        string damageColoured = (skill.Heals && damage >= 0 ? "<color=green>" : (damage != 0 ? "<color=red>" : "<color=grey>")) + damage + "</color>";
        string damageString = " ( " + damageColoured + " )";
        output += user.EntityName + " used " + skill.SkillName + " on " + (skill.IsSelfTargeted ? "themselves" : target.EntityName) + damageString;

        BattleLogMessage message = Instantiate(logPrefab, logParent);
        message.Initialise(output);
    }

    public void Clear()
    {
        logParent.DestroyAllChildren();
    }
}
