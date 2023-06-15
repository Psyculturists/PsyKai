using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectFocus : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TextMeshProUGUI skillNameText;
    [SerializeField]
    private Button useButton;
    [SerializeField]
    private Button cancelButton;

    private Skill selectedSkill;

    public System.Action<Skill> UsedSkillCallback;
    public System.Action CancelCallback;

    private void Awake()
    {
        useButton.onClick.AddListener(UseSelectedSkill);
        cancelButton.onClick.AddListener(Cancel);
    }

    public void AssignSelectedSkill(Skill skill)
    {
        selectedSkill = skill;
        UpdateFields();
    }

    private void UpdateFields()
    {
        if (selectedSkill)
        {
            iconImage.sprite = selectedSkill.Icon;
            skillNameText.text = selectedSkill.SkillName;
        }
        else
        {
            iconImage.sprite = null;
            skillNameText.text = null;
        }
    }

    public void UseSelectedSkill()
    {
        if (selectedSkill != null)
        {
            UsedSkillCallback?.Invoke(selectedSkill);
        }
        // send to combat system
    }

    public void Cancel()
    {
        CancelCallback?.Invoke();
        //close the radial UI
        AssignSelectedSkill(null);
    }
}
