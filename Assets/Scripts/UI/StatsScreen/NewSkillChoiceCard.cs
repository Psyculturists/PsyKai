using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewSkillChoiceCard : MonoBehaviour
{
    [SerializeField]
    private Image highlight;
    [SerializeField]
    private TextMeshProUGUI skillText;
    [SerializeField]
    private Button button;

    private Skill housedSkill;
    public Skill HousedSkill => housedSkill;
    private System.Action<NewSkillChoiceCard> clickAction;

    public void Initialise(Skill skill, System.Action<NewSkillChoiceCard> onClick)
    {
        housedSkill = skill;
        skillText.text = housedSkill.skillName;
        clickAction = onClick;
        button.onClick.AddListener(ClickAction);
    }

    private void OnDestroy()
    {
        clickAction = null;
    }

    private void ClickAction()
    {
        clickAction?.Invoke(this);
    }

    public void SetHighlight(bool highlighted)
    {
        highlight.gameObject.SetActive(highlighted);
    }
}
