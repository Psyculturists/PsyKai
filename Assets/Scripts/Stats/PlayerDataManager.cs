using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    [SerializeField]
    private PlayerEntityData basePlayerData;
    [SerializeField]
    private PlayerUnlockableSkills unlockableSkills;
    public List<Skill> UnlockableSkills => unlockableSkills.PlayerUnlockables;
    [SerializeField]
    private bool doLoad = true;
    [SerializeField]
    private bool overridePlayerLevel = false;
    [SerializeField]
    private int PlayerLevelForOverride = 1;

    private int playerLevel = 1;
    public int PlayerLevel => overridePlayerLevel ? PlayerLevelForOverride : playerLevel;
    private int currentExperience;
    public int CurrentExperience => currentExperience;

    private int maxLevel = 100;
    public int MaxLevel => maxLevel;

    // where do we put this data?
    private int maxSkills = 4;
    public int MaxSkills => maxSkills;

    public List<Skill> currentSkillLoadout = new List<Skill>();
    public List<Skill> CurrentSkillLoadout => currentSkillLoadout;

    public static System.Action ExpGainOccured;

    private const string LEVEL_SAVE_KEY = "PlayerLevel";
    private const string EXP_SAVE_KEY = "PlayerExp";
    private const string SKILLS_SAVE_KEY = "SkillLoadout";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        InitialisePlayerElements();
    }

    private void OnDestroy()
    {
        if(Instance == this)
        ExpGainOccured = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainExp(int expGained)
    {
        Debug.Log("exp gained:  " + expGained);
        currentExperience += expGained;
        (int, int) levelsExp = LevelManager.GetLevelsGainedAndExpRemainder(PlayerLevel, CurrentExperience);
        Debug.Log(levelsExp);
        playerLevel = Mathf.Clamp(playerLevel + levelsExp.Item1, 1, maxLevel);
        currentExperience = levelsExp.Item2;

        ExpGainOccured?.Invoke();
        SaveLevelState();
    }

    public void InitialisePlayerElements()
    {
        // save/load tied in here?


        currentSkillLoadout.Clear();
        foreach(Skill skill in basePlayerData.Skills)
        {
            currentSkillLoadout.Add(skill);
        }

        LoadLevelState();
    }

    public StatData CurrentLevelStats()
    {
        return basePlayerData.StatsForCurrentLevel;
    }

    public void UpdateSkillLoadout(List<Skill> newSkills)
    {
        currentSkillLoadout.Clear();
        foreach(Skill skill in newSkills)
        {
            currentSkillLoadout.Add(skill);
        }
        SaveLevelState();
    }

    private void SaveLevelState()
    {
        if (!doLoad) return;
        ES3.Save(LEVEL_SAVE_KEY, PlayerLevel);
        ES3.Save(EXP_SAVE_KEY, currentExperience);
        ES3.Save(SKILLS_SAVE_KEY, CurrentSkillLoadout);
    }

    private void LoadLevelState()
    {
        if (!doLoad) return;
        playerLevel = ES3.Load(LEVEL_SAVE_KEY, playerLevel);
        currentExperience = ES3.Load(EXP_SAVE_KEY, currentExperience);
        currentSkillLoadout = ES3.Load(SKILLS_SAVE_KEY, currentSkillLoadout);
    }
}
