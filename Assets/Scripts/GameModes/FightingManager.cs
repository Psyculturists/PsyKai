using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingManager : MonoBehaviour
{
    [SerializeField]
    private Transform playerSpawn;
    [SerializeField]
    private PlayerEntityData playerData;
    [SerializeField]
    private Transform enemySpawn;
    [SerializeField]
    private EnemyEntityData tempEnemyData;
    [SerializeField]
    private RadialSkillMenu radialMenu;
    [SerializeField]
    private Button openRadialButton;

    private PlayerEntity spawnedPlayer;
    private Enemy spawnedEnemy;

    private int turnCounter = 1;

    private bool hasInitialised = false;

    // Start is called before the first frame update
    void Awake()
    {

    }

    private void Initialise()
    {
        if (!hasInitialised)
        {
            openRadialButton.onClick.AddListener(ShowRadial);
            radialMenu.SetFocusCallbacks(OnSkillUsed, OnSkillCanceled);
            hasInitialised = true;
        }
    }

    public void OpenFightingScene()
    {
        Initialise();
        EstablishCombatScene();
    }

    private void EstablishCombatScene()
    {
        Cleanup();
        SpawnPlayer();
        SpawnEnemy(tempEnemyData);
        turnCounter = 1;
        HideRadial();
    }

    private void Cleanup()
    {
        if(spawnedPlayer)
        {
            Destroy(spawnedPlayer.gameObject);
        }
        if(spawnedEnemy)
        {
            Destroy(spawnedEnemy.gameObject);
        }
    }

    private void TotalRestart()
    {
        Cleanup();
        EstablishCombatScene();
    }


    private void SpawnPlayer()
    {
        spawnedPlayer = Instantiate(playerData.EntityPrefab, playerSpawn);
        // use current level stats, rather than base stats.
        spawnedPlayer.Initialise(playerData, playerData.StatsForCurrentLevel, playerData.Skills);
        spawnedPlayer.SetName("Psychologist-kun");
    }
    
    private void SpawnEnemy(EnemyEntityData enemyData)
    {
        spawnedEnemy = Instantiate(enemyData.EntityPrefab, enemySpawn);
        spawnedEnemy.Initialise(enemyData, enemyData.BaseStats, enemyData.Skills);
        spawnedEnemy.SetName(enemyData.EntityName);
    }

    public void ShowRadial()
    {
        radialMenu.gameObject.SetActive(true);
        radialMenu.Show(playerData.Skills);
        openRadialButton.gameObject.SetActive(false);
    }

    public void HideRadial()
    {
        radialMenu.gameObject.SetActive(false);
        openRadialButton.gameObject.SetActive(true);
    }

    private void OnSkillUsed(Skill skill)
    {
        skill.OnResolution += OnSkillFinishedFiring;
        spawnedPlayer.CastSkill(skill, spawnedEnemy);
        HideRadial();
        
    }

    private void OnSkillFinishedFiring(bool notCanceled = true)
    {
        if(!notCanceled)
        {
            return;
        }
        if (spawnedEnemy.IsAlive)
        {
            spawnedEnemy.CastSkill(spawnedEnemy.GetRandomSkill(), spawnedPlayer);
        }
        else if(spawnedEnemy.HasBeenSaved)
        {
            EndCombat(true);
            return;
        }
        else if(spawnedEnemy.HasBeenFailed)
        {
            //need another end case for failing to save them?
            EndCombat(false);
            return;
        }

        if (!spawnedPlayer.IsAlive)
        {
            EndCombat(false);
        }
        else
        {
            TriggerNextTurn();
        }
    }

    private void TriggerNextTurn()
    {
        Debug.Log("Next turn triggered");
        turnCounter++;
        spawnedPlayer?.ReduceStatusTimers(1);
        spawnedEnemy?.ReduceStatusTimers(1);
    }

    private void OnSkillCanceled()
    {
        HideRadial();
    }

    private void EndCombat(bool playerVictory)
    {
        Debug.Log("Did player win? " + (playerVictory ? "yes" : "no"));

        if(playerVictory)
        {
            spawnedEnemy.OnDefeat();
        }
        else
        {
            PopupManager.Instance.ShowInfoPopup("Oh Dear :(", "It seems you lost! Try making some food to help out next time! (pending extended game loop)");
        }
        TotalRestart();
        //stuff
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
