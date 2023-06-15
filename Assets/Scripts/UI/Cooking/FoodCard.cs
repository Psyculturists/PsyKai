using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FoodCard : UICard
{
    [SerializeField]
    private TextMeshProUGUI quantityText;

    private FoodItem foodItem;

    public override void SelectCard()
    {
        base.SelectCard();
        CookingUIParent.Instance.SetFoodOnPanel(foodItem);
    }

    public void Init(FoodItem item, int amount)
    {
        foodItem = item;
        quantityText.text = amount.ToString();
    }

}
