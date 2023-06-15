using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    private static Dictionary<ItemData, int> OwnedItems;
    private static Dictionary<ItemData, int> recipeInventory = new Dictionary<ItemData, int>();
    public static Dictionary<ItemData, int> RecipeInventory => recipeInventory;
    private static Dictionary<ItemData, int> seedInventory = new Dictionary<ItemData, int>();
    public static Dictionary<ItemData, int> SeedInventory => seedInventory;
    private static Dictionary<ItemData, int> ingredientInventory = new Dictionary<ItemData, int>();
    public static Dictionary<ItemData, int> IngredientInventory => ingredientInventory;

    private static Dictionary<FoodItem, int> foodInventory = new Dictionary<FoodItem, int>();
    public static Dictionary<FoodItem, int> FoodInventory => foodInventory;

    private static Dictionary<ItemData, int> baitInventory = new Dictionary<ItemData, int>();
    public static Dictionary<ItemData, int> BaitInventory => baitInventory;

    private static Dictionary<ItemData, int> GetRelativeInventory(ItemData item)
    {
        if(item is RecipeData)
        {
            return recipeInventory;
        }
        else if(item is SeedData)
        {
            return seedInventory;
        }
        else if(item is IngredientData)
        {
            return ingredientInventory;
        }
        else if(item is BaitData)
        {
            return baitInventory;
        }

        Debug.LogError("Item is not of a required type. Dynamic inventory failed.");
        return null;
    }

    public static int QuantityOwned(ItemData item)
    {
        GetRelativeInventory(item).TryGetValue(item, out int val);
        if(val == -1)
        {
            return 0;
        }
        return val;
    }
    public static int QuantityOwned(FoodItem item)
    {
        if(item == null)
        {
            return 0;
        }
        foodInventory.TryGetValue(item, out int val);
        if (val == -1)
        {
            return 0;
        }
        return val;
    }

    public static bool OwnsAtLeast(ItemData item, int quantity)
    {
        return QuantityOwned(item) >= quantity;
    }

    public static bool OwnsAtLeast(FoodItem item, int quantity)
    {
        return QuantityOwned(item) >= quantity;
    }

    private static void Consume(ItemData item, int quantity)
    {
        GetRelativeInventory(item)[item] -= quantity;
    }

    private static void Consume(FoodItem item, int quantity)
    {
        foodInventory[item] -= quantity;
    }

    public static bool UseItems(ItemData item, int quantity)
    {
        if(OwnsAtLeast(item, quantity))
        {
            Consume(item, quantity);
            return true;
        }
        return false;
    }

    public static bool UseItems(Dictionary<ItemData, int> items)
    {
        foreach(var itemPair in items)
        {
            if(OwnsAtLeast(itemPair.Key, itemPair.Value))
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        foreach(var itemPair in items)
        {
            UseItems(itemPair.Key, itemPair.Value);
        }
        return true;
    }

    public static bool UseFood(FoodItem item, int quantity)
    {
        if(OwnsAtLeast(item, quantity))
        {
            Consume(item, quantity);
            return true;
        }
        return false;
    }

    public static bool GrantItem(ItemData item, int quantity)
    {
        Dictionary<ItemData, int> relativeInventory = GetRelativeInventory(item);
        if (relativeInventory.ContainsKey(item))
        {
            relativeInventory[item] += quantity;
        }
        else
        {
            relativeInventory.Add(item, quantity);
        }
        return true;
    }

    public static bool GrantItem(FoodItem item, int quantity)
    {
        if (foodInventory.ContainsKey(item))
        {
            foodInventory[item] += quantity;
        }
        else
        {
            foodInventory.Add(item, quantity);
        }
        return true;
    }
}
