using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe Data", menuName = "Data/Recipe")]
public class RecipeData : ItemData
{
    [SerializeField]
    private IngredientAmountDictionary ingredients = new IngredientAmountDictionary();
    public IngredientAmountDictionary Ingredients => ingredients;

    [SerializeField]
    private StatData statContributions;
    public StatData StatContributions => statContributions;

    [SerializeField]
    private StatusEffectDurationDictionary statusEffectsOnUse = new StatusEffectDurationDictionary();
    public StatusEffectDurationDictionary StatusEffectsOnUse => statusEffectsOnUse;

    [SerializeField]
    private FoodItem result;
    public FoodItem Result => result;


    public bool PlayerHasRequiredIngredients()
    {
        foreach(var pair in Ingredients)
        {
            if(!Inventory.OwnsAtLeast(pair.Key, pair.Value))
            {
                return false;
            }
        }
        return true;
    }
}
