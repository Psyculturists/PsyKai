using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : Item
{
    private RecipeData Recipe => data as RecipeData;

    public FoodItem()
    {

    }

    public FoodItem(RecipeData data)
    {
        this.data = data;
    }

    public bool Consume()
    {
        return Inventory.UseFood(this, 1);
    }

    public StatData GetStatData()
    {
        return Recipe.StatContributions;
    }

    public RecipeData GetRecipe()
    {
        return Recipe;
    }
}
