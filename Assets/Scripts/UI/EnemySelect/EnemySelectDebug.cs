using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectDebug : MonoBehaviour
{
    [SerializeField]
    private GameObject root;
    [SerializeField]
    private EnemySelectButton enemyButtonPrefab;
    [SerializeField]
    private Transform enemyParent;

    [SerializeField]
    private List<EnemyEntityData> enemyList;
    [SerializeField]
    private List<EnemyGroup> enemyGroupList;

    private List<EnemySelectButton> spawnedButtons = new List<EnemySelectButton>();

    public void ToggleOpen()
    {
        if (root.activeSelf)
        {
            Close();
        }
        else
        {
            OpenSelect();
        }
    }

    public void OpenSelect()
    {
        root.SetActive(true);
        SpawnCards();
    }

    public void Close()
    {
        enemyParent.DestroyAllChildren();
        spawnedButtons.Clear();
        root.SetActive(false);
    }

    private void SpawnCards()
    {
        foreach(EnemyEntityData data in enemyList)
        {
            EnemySelectButton enemy = Instantiate(enemyButtonPrefab, enemyParent);
            enemy.Initialise(data, EnterCombatWithEnemy);
            spawnedButtons.Add(enemy);
        }
        foreach(EnemyGroup group in enemyGroupList)
        {
            EnemySelectButton enemy = Instantiate(enemyButtonPrefab, enemyParent);
            enemy.Initialise(group, EnterCombatWithEnemies);
            spawnedButtons.Add(enemy);
        }
    }

    private void EnterCombatWithEnemy(EnemyEntityData enemyData)
    {
        FightingManager.Instance.OpenFightingForEnemy(enemyData);
        Close();
    }
    private void EnterCombatWithEnemies(List<EnemyEntityData> enemyData)
    {
        FightingManager.Instance.OpenFightingForEnemy(enemyData);
        Close();
    }
}
