using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Flee Skill", menuName = "Data/Flee Skill")]
public class FleeSkill : Skill
{

    public override void AlternativeCastEffect(CombatEntity target)
    {
        base.AlternativeCastEffect(target);
        ResolveSkill(true);
        NavigationBar.Instance.OpenHub();
    }
}
