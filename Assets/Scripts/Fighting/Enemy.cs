using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatEntity
{
    [SerializeField]
    private DropTable rewardTable;
    public DropTable RewardTable => rewardTable;

    public void OnDefeat()
    {

    }

    public void PlayDefeatedLine()
    {
        //NaniNovelTest.Instance.StartNaniNovelSequence();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
