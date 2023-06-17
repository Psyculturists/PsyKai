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

    private int playerLevel = 1;
    public int PlayerLevel => playerLevel;
    private int currentExperience;
    public int CurrentExperience => currentExperience;

    private int maxLevel = 100;
    public int MaxLevel => maxLevel;

    // where do we put this data?
    private int maxSkills = 4;
    public int MaxSkills => maxSkills;

    public List<Skill> currentSkillLoadout = new List<Skill>();
    public List<Skill> CurrentSkillLoadout => currentSkillLoadout;

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
    }

    public void InitialisePlayerElements()
    {
        // save/load tied in here?


        currentSkillLoadout.Clear();
        foreach(Skill skill in basePlayerData.Skills)
        {
            currentSkillLoadout.Add(skill);
        }
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
    }
}
