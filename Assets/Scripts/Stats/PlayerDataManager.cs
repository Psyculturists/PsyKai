using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    [SerializeField]
    private PlayerEntityData basePlayerData;

    private int playerLevel = 1;
    public int PlayerLevel => playerLevel;
    private int currentExperience;
    public int CurrentExperience => currentExperience;

    private int maxLevel = 100;
    public int MaxLevel => maxLevel;

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
}
