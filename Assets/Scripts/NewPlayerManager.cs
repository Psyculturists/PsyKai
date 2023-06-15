using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerManager : MonoBehaviour
{
    public static NewPlayerManager Instance;

    [SerializeField]
    private RecipeData[] StartingRecipes;
    [SerializeField]
    private SeedData[] StartingSeeds;
    [SerializeField]
    private int amountOfBait;
    [SerializeField]
    private BaitData startingBaitType;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        foreach(RecipeData data in StartingRecipes)
        {
            Inventory.GrantItem(data, 1);
        }
        Inventory.GrantItem(startingBaitType, amountOfBait);
    }
}
