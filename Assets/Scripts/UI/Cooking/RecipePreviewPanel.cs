using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipePreviewPanel : MonoBehaviour
{
    [SerializeField]
    private Image previewImage;
    [SerializeField]
    private RecipeBenefitUI benefitPrefab;
    [SerializeField]
    private Transform benefitsParent;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private TextMeshProUGUI buttonText;
    [SerializeField]
    private Button cancelButton;

    private RecipeData selectedRecipe;
    private FoodItem selectedFood;

    private bool showingFood = false;

    private void Awake()
    {
        createButton.onClick.AddListener(TryCreate);
        cancelButton.onClick.AddListener(Cancel);
    }

    public void SetRecipe(RecipeData data)
    {
        selectedRecipe = data;
        UpdateUI();
    }
    public void SetFood(FoodItem item)
    {
        selectedFood = item;
    }
    public void SetForFoodState(bool state)
    {
        cancelButton.gameObject.SetActive(state);
        showingFood = state;
        buttonText.text = showingFood ? "Use" : "Create";
        UpdateButtonAvailability();
    }

    private void UpdateUI()
    {
        previewImage.sprite = selectedRecipe.Icon;
        benefitsParent.DestroyAllChildren();
        SpawnBenefits();
        createButton.interactable = selectedRecipe.PlayerHasRequiredIngredients();
    }

    private void UpdateButtonAvailability()
    {
        if(!showingFood)
        {
            createButton.interactable = true;
        }
        else
        {
            createButton.interactable = Inventory.FoodInventory.Count > 0;
        }
    }

    private void SpawnBenefits()
    {
        SpawnStats(selectedRecipe.StatContributions);

        foreach(var ing in selectedRecipe.StatusEffectsOnUse)
        {
            SpawnForStatus(ing.Key, ing.Value);
        }
    }

    private void SpawnStats(StatData data)
    {
        if(data.Attack != 0)
        {
            SpawnForStat(StatType.Attack, data.Attack);
        }
        if(data.Defence != 0)
        {
            SpawnForStat(StatType.Defence, data.Defence);
        }
        if(data.Speed != 0)
        {
            SpawnForStat(StatType.Speed, data.Speed);
        }
    }

    private void SpawnForStat(StatType stat, int value)
    {
        RecipeBenefitUI benefitUI = Instantiate(benefitPrefab, benefitsParent);
        benefitUI.SetForStat(stat, value);
    }

    private void SpawnForStatus(StatusEffect effect, int duration)
    {
        foreach(var eff in effect.StatsImpacted)
        {
            RecipeBenefitUI benefitUI = Instantiate(benefitPrefab, benefitsParent);
            benefitUI.SetForStatus(effect, eff, duration);
        }
    }


    public void TryCreate()
    {
        if(selectedRecipe == null)
        {
            PopupManager.Instance.ShowInfoPopup("No Recipe Selected", "You need to select a recipe!");
            return;
        }

        if(showingFood)
        {
            TryUse();
            return;
        }

        if(selectedRecipe.PlayerHasRequiredIngredients())
        {
            foreach(var pair in selectedRecipe.Ingredients)
            {
                Inventory.UseItems(pair.Key, pair.Value);
            }
            Inventory.GrantItem(new FoodItem(selectedRecipe), 1);
        }
        else
        {
            PopupManager.Instance.ShowInfoPopup("No Ingredients :(", "You should get some ingredients to make this.");
        }
    }

    public void Cancel()
    {
        NavigationBar.Instance.ReturnFromFoodToBattle();
        CookingUIParent.Instance.OnExitedWithoutUse?.Invoke();
    }

    public void TryUse()
    {
        Inventory.UseFood(selectedFood, 1);
        NavigationBar.Instance.ReturnFromFoodToBattle();
        CookingUIParent.Instance.OnUsedFood?.Invoke(selectedFood);
    }

}
