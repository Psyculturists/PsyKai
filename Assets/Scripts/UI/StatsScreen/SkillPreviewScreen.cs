using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillPreviewScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Button acceptButton;

    private Skill selectedSkill;

    public void SetAcceptFunctionality(UnityEngine.Events.UnityAction action)
    {
        acceptButton.onClick.AddListener(action);
    }

    public void AssignSkill(Skill newSkill)
    {
        selectedSkill = newSkill;
        UpdateElements();
    }

    private void UpdateElements()
    {
        nameText.text = selectedSkill == null ? "None" : selectedSkill.SkillName;
        descriptionText.text = selectedSkill == null ? "Select a skill" : selectedSkill.Description;
    }
}
