using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelExpDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Slider expSlider;
    [SerializeField]
    private TextMeshProUGUI expText;

    private bool waitUpdate = false;

    private void OnEnable()
    {
        if(PlayerDataManager.Instance == null)
        {
            waitUpdate = true;
            return;
        }
        else
        {
            UpdateFields();
        }
    }

    private void UpdateFields()
    {
        int currentExp = PlayerDataManager.Instance.CurrentExperience;
        int nextExp = LevelManager.GetXPRequiredForNextLevel(PlayerDataManager.Instance.PlayerLevel);
        float ratio = (float)currentExp / (float)nextExp;

        levelText.text = PlayerDataManager.Instance.PlayerLevel.ToString();
        expText.text = PlayerDataManager.Instance.CurrentExperience.ToString() + " / " + LevelManager.GetXPRequiredForNextLevel(PlayerDataManager.Instance.PlayerLevel).ToString();
        expSlider.value = ratio;
    }

    private void Update()
    {
        if(waitUpdate)
        {
            if(PlayerDataManager.Instance != null)
            {
                UpdateFields();
                waitUpdate = false;
            }
        }
    }
}
