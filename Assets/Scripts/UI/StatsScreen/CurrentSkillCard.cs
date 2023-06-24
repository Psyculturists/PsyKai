using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentSkillCard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI skillNumberText;
    [SerializeField]
    private TextMeshProUGUI skillNameText;
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image highlightImage;

    private Skill housedSkill;
    public Skill HousedSkill => housedSkill;
    private System.Action<CurrentSkillCard> onClickAction = null;

    private void Awake()
    {
        button.onClick.AddListener(OnInteracted);
    }

    private void OnDestroy()
    {
        onClickAction = null;
    }

    public void Init(int numberForSlot, Skill skill, System.Action<CurrentSkillCard> clickEvent)
    {
        housedSkill = skill;
        skillNumberText.text = numberForSlot.ToString();
        Populate();
        onClickAction = clickEvent;
    }

    private void Populate()
    {
        skillNameText.text = housedSkill == null ? "None" : housedSkill.skillName;
    }

    private void OnInteracted()
    {
        onClickAction?.Invoke(this);
    }

    public void SetHighlight(bool selected)
    {
        if(gameObject)
        highlightImage.gameObject.SetActive(selected);
    }
}
