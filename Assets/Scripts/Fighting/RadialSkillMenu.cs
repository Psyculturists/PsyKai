using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSkillMenu : MonoBehaviour
{
    [SerializeField]
    private Transform radialParent;
    [SerializeField]
    private SkillSelectFocus focus;
    [SerializeField]
    private List<BattleSkillButton> skillButtons;


    public void Show(List<Skill> skillList, CombatEntity target)
    {
        for(int i = 0; i < skillButtons.Count; i++)
        {
            if(skillList.Count > i)
            {
                skillButtons[i].AssignSkill(skillList[i]);
            }
            else
            {
                skillButtons[i].AssignSkill(null);
            }
            skillButtons[i].UpdateResistanceIndication(target);
        }
    }

    public void ToggleInteractivity(bool state)
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].SetInteractive(state);
        }
    }

    private void FocusOnSkill(Skill skill)
    {
        focus.AssignSelectedSkill(skill);
    }

    public void SetFocusCallbacks(System.Action<Skill> useCallback, System.Action cancelCallback)
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].SetCallback(useCallback);
        }
        //focus.UsedSkillCallback = useCallback;
        //focus.CancelCallback = cancelCallback;
    }

    // Start is called before the first frame update
    void Awake()
    {
        //SetCallbackOnRadials();
    }

    private void OnEnable()
    {
        FocusOnSkill(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
