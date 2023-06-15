using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientNeededDisplay : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TextMeshProUGUI ingredientText;

    public void SetForIngredient(IngredientData ingredient, int amount)
    {
        iconImage.sprite = ingredient.Icon;
        ingredientText.text = amount + "x " + ingredient.ItemName;
    }
}
