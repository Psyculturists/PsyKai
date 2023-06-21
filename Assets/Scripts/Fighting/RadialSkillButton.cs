using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadialSkillButton : MonoBehaviour
{
    [SerializeField]
    private Image skillImage;
    [SerializeField]
    private TextMeshProUGUI skillText;
    [SerializeField]
    private Button button;

    [SerializeField]
    private Skill loadedSkill;

    private System.Action<Skill> clickCallback;

    private void Awake()
    {
        if(loadedSkill)
        {
            SetFields();
        }
        button.onClick.AddListener(TryEvoke);
    }

    private void OnDestroy()
    {
        clickCallback = null;
    }


    public void AssignSkill(Skill newSkill)
    {
        loadedSkill = newSkill;
        SetFields();
    }

    public void SetCallback(System.Action<Skill> callback)
    {
        clickCallback = callback;
    }

    private void TryEvoke()
    {
        clickCallback?.Invoke(loadedSkill);
    }

    private void SetFields()
    {
        if (loadedSkill)
        {
            skillImage.sprite = loadedSkill.Icon;
            skillText.text = loadedSkill.skillName;
        }
        else
        {
            skillImage.sprite = null;
            skillText.text = null;
        }
    }
}
