using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : Item
{
    public Recipe()
    {

    }

    public Recipe(RecipeData startingData)
    {
        data = startingData;
    }
}
