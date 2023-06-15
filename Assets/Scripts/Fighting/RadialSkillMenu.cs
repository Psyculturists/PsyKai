using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSkillMenu : MonoBehaviour
{
    [SerializeField]
    private Transform radialParent;
    [SerializeField]
    private SkillSelectFocus focus;
    [SerializeField]
    private List<RadialSkillButton> skillButtons;


    public void Show(List<Skill> skillList)
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
        }
    }

    private void FocusOnSkill(Skill skill)
    {
        focus.AssignSelectedSkill(skill);
    }

    private void SetCallbackOnRadials()
    {
        foreach(RadialSkillButton but in skillButtons)
        {
            but.SetCallback(FocusOnSkill);
        }
    }

    public void SetFocusCallbacks(System.Action<Skill> useCallback, System.Action cancelCallback)
    {
        focus.UsedSkillCallback = useCallback;
        focus.CancelCallback = cancelCallback;
    }

    // Start is called before the first frame update
    void Awake()
    {
        SetCallbackOnRadials();
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
