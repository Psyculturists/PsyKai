using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingManager : MonoBehaviour
{
    public static FightingManager Instance;


    [SerializeField]
    private GameObject root;
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
    private Enemy targetedEnemy;
    //currently spawn locations might bug out if respawning enemies or summons
    private List<Enemy> spawnedEnemies = new List<Enemy>();
    [SerializeField]
    private List<Transform> enemySpawnPoints = new List<Transform>();

    private int turnCounter = 1;

    private bool hasInitialised = false;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
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

    public void CloseFightingScene()
    {
        Cleanup();
        root.gameObject.SetActive(false);
    }

    public void OpenFightingForEnemy(EnemyEntityData data)
    {
        root.gameObject.SetActive(true);
        Initialise();
        EstablishCombatScene(data);
    }

    public void OpenFightingForEnemy(List<EnemyEntityData> enemyData)
    {
        root.gameObject.SetActive(true);
        Initialise();
        EstablishCombatScene(enemyData);
    }

    private void EstablishCombatScene(EnemyEntityData data = null)
    {
        Cleanup();
        SpawnPlayer();
        SpawnEnemy(data == null ? tempEnemyData : data);
        turnCounter = 1;
        HideRadial();
    }

    private void EstablishCombatScene(List<EnemyEntityData> data = null)
    {
        Cleanup();
        SpawnPlayer();
        if(data == null)
        {
            SpawnEnemy(tempEnemyData);
        }
        else
        {
            foreach(EnemyEntityData d in data)
            {
                SpawnEnemy(d);
            }
        }
        turnCounter = 1;
        HideRadial();
    }

    private void Cleanup()
    {
        if(spawnedPlayer)
        {
            Destroy(spawnedPlayer.gameObject);
        }
        foreach(Transform t in enemySpawnPoints)
        {
            t.DestroyAllChildren();
        }
        spawnedEnemies.Clear();
    }

    private void TotalRestart()
    {
        Cleanup();
    }


    private void SpawnPlayer()
    {
        spawnedPlayer = Instantiate(playerData.EntityPrefab, playerSpawn);
        // use current level stats, rather than base stats.
        spawnedPlayer.Initialise(playerData, playerData.StatsForCurrentLevel, PlayerDataManager.Instance.CurrentSkillLoadout);
        spawnedPlayer.SetName("Psychologist-kun");
    }
    
    private void SpawnEnemy(EnemyEntityData enemyData)
    {
        Transform parentTransform = enemySpawnPoints[spawnedEnemies.Count];

        Enemy spawnedEnemy = Instantiate(enemyData.EntityPrefab, parentTransform);
        spawnedEnemy.Initialise(enemyData, enemyData.BaseStats, enemyData.Skills, SelectTarget);
        spawnedEnemy.SetName(enemyData.EntityName);
        spawnedEnemies.Add(spawnedEnemy);
        if(spawnedEnemies.Count == 1 && targetedEnemy == null)
        {
            SelectTarget(spawnedEnemies[0]);
        }
    }

    public void ShowRadial()
    {
        radialMenu.gameObject.SetActive(true);
        radialMenu.Show(spawnedPlayer.Skills);
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
        spawnedPlayer.CastSkill(skill, targetedEnemy);
        HideRadial();
        
    }

    private void OnSkillFinishedFiring(bool notCanceled = true)
    {
        if(!notCanceled)
        {
            return;
        }
        List<Enemy> enemiesSavedThisRound = new List<Enemy>();
        foreach (Enemy enemy in spawnedEnemies)
        {
            if (enemy.IsAlive)
            {
                enemy.CastSkill(enemy.GetRandomSkill(), spawnedPlayer);
            }
            else if (enemy.HasBeenSaved)
            {
                enemiesSavedThisRound.Add(enemy);
                continue;
            }
            else if (enemy.HasBeenFailed)
            {
                //need another end case for failing to save them?
                EndCombat(false);
                return;
            }
        }
        foreach(Enemy enemy in enemiesSavedThisRound)
        {
            OnEnemySaved(enemy);
            // make them non-targets, or remove them?
        }
        if(spawnedEnemies.Count == 0)
        {
            EndCombat(true);
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

    private void OnEnemySaved(Enemy enemy)
    {
        enemy.OnDefeat();
        spawnedEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void TriggerNextTurn()
    {
        Debug.Log("Next turn triggered");
        turnCounter++;
        spawnedPlayer?.ReduceStatusTimers(1);
        foreach (Enemy enemy in spawnedEnemies)
        {
            enemy?.ReduceStatusTimers(1);
        }
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
            GetCombatRewards();
        }
        else
        {
            PopupManager.Instance.ShowInfoPopup("Oh Dear :(", "It seems you lost! Try making some food to help out next time! (pending extended game loop)", () => NavigationBar.Instance.OpenHub());
        }
        TotalRestart();
        //stuff
    }

    private void OnVictoryCallback()
    {
        CloseFightingScene();
    }

    private void GetCombatRewards()
    {
        Dictionary<ItemData, int> rewardsToGet = new Dictionary<ItemData, int>();
        int expToGet = 0;
        foreach (Enemy e in spawnedEnemies)
        {
            // drops can probably happen in more than multiples of 1?
            ItemData reward = e.RewardTable.Evaluate();
            Inventory.GrantItem(reward, 1);
            if (rewardsToGet.ContainsKey(reward))
            {
                rewardsToGet[reward] = rewardsToGet[reward] + 1;
            }
            else
            {
                rewardsToGet[reward] = 1;
            }
            expToGet += e.ExpForDefeat;
        }
        string rewardsAsStrings = "";

        int rewardsAdded = 1;
        foreach(var pair in rewardsToGet)
        {
            rewardsAsStrings += pair.Key.ItemName + " x" + pair.Value + (rewardsAdded < rewardsToGet.Count ? ", " : "!");
            rewardsAdded++;
        }
        //PlayDefeatedLine();
        PopupManager.Instance.ShowInfoPopup("Reward Obtained!", "You just gained " + rewardsAsStrings + "!", () => CloseFightingScene());
        //temp line
        PlayerDataManager.Instance.GainExp(expToGet);
    }

    private void SelectTarget(CombatEntity entity)
    {
        bool selectingCurrent = targetedEnemy == entity;

        if(!selectingCurrent && targetedEnemy != null)
        {
            targetedEnemy.ToggleIndicator(false);
        }
        targetedEnemy = entity as Enemy;
        if(!selectingCurrent)
        {
            targetedEnemy.ToggleIndicator(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
