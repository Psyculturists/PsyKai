using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RecipeCard : UICard
{
    [SerializeField]
    private Transform ingredientListParent;
    [SerializeField]
    private IngredientNeededDisplay ingredientDisplayPrefab;

    private RecipeData recipeData => Data as RecipeData;
    
    public override void SetupCard(ItemData inData, RecipePreviewPanel panel, Action<UICard> clearAction)
    {
        base.SetupCard(inData, panel, clearAction);
        CreateIngredientList();
    }
    
    public void CreateIngredientList()
    {
        foreach(var v in recipeData.Ingredients)
        {
            IngredientNeededDisplay display = Instantiate(ingredientDisplayPrefab, ingredientListParent);
            display.SetForIngredient(v.Key, v.Value);
        }
    }
}
