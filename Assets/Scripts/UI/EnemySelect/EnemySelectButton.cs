using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemySelectButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI enemyNameText;
    [SerializeField]
    private TextMeshProUGUI enemyLevelText;
    [SerializeField]
    private Button button;

    private EnemyEntityData data;
    private EnemyGroup groupData;
    private System.Action<EnemyEntityData> clickEffect;
    private System.Action<List<EnemyEntityData>> groupClickEffect;

    public void Initialise(EnemyEntityData enemyData, System.Action<EnemyEntityData> click)
    {
        data = enemyData;
        enemyNameText.text = data.EntityName;
        enemyLevelText.text = "Lvl. " + data.SuggestedLevel.ToString();
        clickEffect = click;
        button.onClick.AddListener(ClickAction);
    }
    public void Initialise(EnemyGroup enemyData, System.Action<List<EnemyEntityData>> click)
    {
        groupData = enemyData;
        enemyNameText.text = groupData.GroupName;
        enemyLevelText.text = "Lvl. " + groupData.SuggestedLevel.ToString();
        groupClickEffect = click;
        button.onClick.AddListener(ClickAction);
    }

    private void OnDestroy()
    {
        clickEffect = null;
    }

    private void ClickAction()
    {
        clickEffect?.Invoke(data);
        groupClickEffect?.Invoke(groupData.Enemies);
    }
}
