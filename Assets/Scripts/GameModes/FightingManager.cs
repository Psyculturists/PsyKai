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
    private BattleSkillMenu skillMenu;
    [SerializeField]
    private BattleLog battleLog;
    [SerializeField]
    private float turnTimer = 7.5f;
    [SerializeField]
    private Button fleeButton;

    private PlayerEntity spawnedPlayer;
    private Enemy targetedEnemy;
    private CombatEntity currentTurnEntity;
    //currently spawn locations might bug out if respawning enemies or summons
    private List<Enemy> spawnedEnemies = new List<Enemy>();

    // snapshot enemies on start of each round, to handle turn cycling - and track turn index
    private List<CombatEntity> startTurnEntitySnapshot = new List<CombatEntity>();
    private int entityTurnIndexer = 0;

    [SerializeField]
    private List<Transform> enemySpawnPoints = new List<Transform>();

    private int turnCounter = 1;
    Dictionary<ItemData, int> rewardsToGet = new Dictionary<ItemData, int>();
    private int currentFightExp = 0;

    private bool hasInitialised = false;

    public bool isFighting => root.gameObject.activeInHierarchy;

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
            skillMenu.SetFocusCallbacks(OnSkillUsed, OnSkillCanceled);
            hasInitialised = true;
            fleeButton.onClick.AddListener(CloseFightingScene);
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
        DisallowSkills();
        currentFightExp = 0;
        rewardsToGet.Clear();

        currentTurnEntity = spawnedPlayer;
        SetupEntitySnapshot();
        SetCombatUILock();
    }

    private void EstablishCombatScene(List<EnemyEntityData> data = null)
    {
        Cleanup();
        SpawnPlayer();
        // commenting out speed verification because summons would mess with it probably...
        //CombatEntity highestSpeedEntity = spawnedPlayer;
        if(data == null)
        {
            CombatEntity enemy = SpawnEnemy(tempEnemyData);
            //highestSpeedEntity = enemy.Speed > highestSpeedEntity.Speed ? enemy : highestSpeedEntity;
        }
        else
        {
            foreach(EnemyEntityData d in data)
            {
                CombatEntity enemy = SpawnEnemy(d);
                //highestSpeedEntity = enemy.Speed > highestSpeedEntity.Speed ? enemy : highestSpeedEntity;
            }
        }
        currentTurnEntity = spawnedPlayer; //should set to highest speed entity later, when figured out.
        turnCounter = 1;
        DisallowSkills();
        currentFightExp = 0;
        rewardsToGet.Clear();
        SetupEntitySnapshot();
        SetCombatUILock();
    }

    private void Cleanup()
    {
        currentTurnEntity = null;
        entityTurnIndexer = 0;
        battleLog.Clear();
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
        spawnedPlayer.SetEndTurnAction(ProceedNextTurn);
    }
    
    private CombatEntity SpawnEnemy(EnemyEntityData enemyData)
    {
        Transform parentTransform = enemySpawnPoints[spawnedEnemies.Count];

        Enemy spawnedEnemy = Instantiate(enemyData.EntityPrefab, parentTransform);
        spawnedEnemy.Initialise(enemyData, enemyData.BaseStats, enemyData.Skills, SelectTarget);
        spawnedEnemy.SetName(enemyData.EntityName);
        spawnedEnemy.SetEndTurnAction(ProceedNextTurn);
        spawnedEnemies.Add(spawnedEnemy);
        if(spawnedEnemies.Count == 1 && targetedEnemy == null)
        {
            SelectTarget(spawnedEnemies[0]);
        }
        return spawnedEnemy;
    }

    public void AllowSkills()
    {
        skillMenu.ToggleInteractivity(true);
        skillMenu.Show(spawnedPlayer.Skills, targetedEnemy);
    }

    public void DisallowSkills()
    {
        skillMenu.ToggleInteractivity(false);
    }

    private void SetCombatUILock()
    {
        if(currentTurnEntity == spawnedPlayer && !currentTurnEntity.HavingTurn)
        {
            AllowSkills();
        }
        else
        {
            DisallowSkills();
        }
    }

    private void OnSkillUsed(Skill skill)
    {
        if (skill == null) return;
        currentTurnEntity.StartEntityTurn(skill, targetedEnemy);
        SetCombatUILock();
    }

    private void OnAnyTurnEnded()
    {
        List<Enemy> enemiesSavedThisRound = new List<Enemy>();
        foreach (Enemy enemy in spawnedEnemies)
        {
            if (enemy.IsAlive)
            {
                //enemy alive
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
    }

    private void ProceedNextTurn()
    {
        OnAnyTurnEnded();

        //stuff
        CombatEntity EntityAtIndex()
        {
            return startTurnEntitySnapshot[entityTurnIndexer];
        };
        if(startTurnEntitySnapshot.Count > 0)
        {
            if (entityTurnIndexer == startTurnEntitySnapshot.Count - 1) entityTurnIndexer = 0;
            else entityTurnIndexer++;

            int indexesChecked = 0;
            
            while (EntityAtIndex()?.IsAlive == false && indexesChecked < (startTurnEntitySnapshot.Count))
            {
                indexesChecked++;
                if (entityTurnIndexer == startTurnEntitySnapshot.Count - 1) entityTurnIndexer = 0;
                else entityTurnIndexer++;
            }

            currentTurnEntity = EntityAtIndex();
        }

        if(currentTurnEntity == null)
        {
            // log an error and make us lose to motivate us to fix it.
            Debug.LogError("Never found suitable turn entity. What happened?");
            EndCombat(false);
        }
        else if(currentTurnEntity != spawnedPlayer)
        {
            currentTurnEntity.StartEntityTurn(currentTurnEntity.GetRandomSkill(), spawnedPlayer);
        }
        SetCombatUILock();
    }

    private void OnEnemySaved(Enemy enemy)
    {
        enemy.OnDefeat();
        currentFightExp += enemy.ExpForDefeat;

        ItemData reward = enemy.RewardTable.Evaluate();
        if (rewardsToGet.ContainsKey(reward))
        {
            rewardsToGet[reward] = rewardsToGet[reward] + 1;
        }
        else
        {
            rewardsToGet[reward] = 1;
        }

        spawnedEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void TriggerNextTurn()
    {
        Debug.Log("Next turn triggered");
        turnCounter++;
        spawnedPlayer?.ReduceStatusTimers(1);

        // create unit snapshot
        foreach (Enemy enemy in spawnedEnemies)
        {
            enemy?.ReduceStatusTimers(1);
        }
        SetupEntitySnapshot();
    }

    private void SetupEntitySnapshot()
    {
        startTurnEntitySnapshot.Clear();
        startTurnEntitySnapshot.Add(spawnedPlayer as CombatEntity);
        foreach (Enemy enemy in spawnedEnemies)
        {
            if (enemy.IsAlive)
            {
                startTurnEntitySnapshot.Add(enemy as CombatEntity);
            }
        }
    }

    private void OnSkillCanceled()
    {
        DisallowSkills();
    }

    public void LogBattleMessage(CombatEntity user, CombatEntity target, Skill skill, int damage)
    {
        battleLog.CreateLog(user, target, skill, damage);
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
            PopupManager.Instance.ShowInfoPopup("Oh Dear :(", "It seems you lost! Try making some food to help out next time! (pending extended game loop)", () => CloseFightingScene());
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
        string rewardsAsStrings = "";

        int rewardsAdded = 1;
        foreach(var pair in rewardsToGet)
        {
            Inventory.GrantItem(pair.Key, pair.Value);
            rewardsAsStrings += pair.Key.ItemName + " x" + pair.Value + (rewardsAdded < rewardsToGet.Count ? ", " : "!");
            rewardsAdded++;
        }
        rewardsToGet.Clear();
        //PlayDefeatedLine();
        PlayerDataManager.Instance.GainExp(currentFightExp);
        currentFightExp = 0;
        PopupManager.Instance.ShowInfoPopup("Reward Obtained!", "You just gained " + rewardsAsStrings + "!", () => CloseFightingScene());
        //temp line
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

        AllowSkills();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
