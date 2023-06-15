using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingUIParent : MonoBehaviour
{
    public static CookingUIParent Instance;

    [SerializeField]
    private RecipePreviewPanel previewPanel;
    [SerializeField]
    private RecipeCard recipeCardPrefab;
    [SerializeField]
    private FoodCard foodCardPrefab;
    [SerializeField]
    private Transform recipeCardParent;
    [SerializeField]
    private Button exitButton;


    private UICard currentlySelectedCard;
    private int selectedIndex = 0;

    private List<UICard> spawnedCards = new List<UICard>();

    public System.Action OnExitedWithoutUse;
    public System.Action<FoodItem> OnUsedFood;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        exitButton.onClick.AddListener(Exit);
    }

    public void Exit()
    {
        NavigationBar.Instance.OpenHub();
    }

    public void OpenForRecipes()
    {
        Clear();
        SpawnRecipes();
        TrySelectFirstCard();
        previewPanel.SetForFoodState(false);
    }

    public void OpenForFood()
    {
        Clear();
        SpawnFoodItems();
        TrySelectFirstCard();
        previewPanel.SetForFoodState(true);
    }

    private void OnDisable()
    {
        Clear();
    }

    private void Clear()
    {
        currentlySelectedCard = null;
        recipeCardParent.DestroyAllChildren();
        spawnedCards.Clear();
    }

    private void SpawnRecipes()
    {
        foreach(var recipePair in Inventory.RecipeInventory)
        {
            if(recipePair.Value > 0)
            {
                spawnedCards.Add(CreateRecipeCard(recipePair.Key as RecipeData));
            }
        }
    }

    private void SpawnFoodItems()
    {
        foreach(var foodItem in Inventory.FoodInventory)
        {
            if(foodItem.Value > 0)
            {
                spawnedCards.Add(CreateFoodCard(foodItem.Key));
            }
        }
    }

    private RecipeCard CreateRecipeCard(RecipeData recipe)
    {
        RecipeCard card = Instantiate(recipeCardPrefab, recipeCardParent);
        card.SetupCard(recipe, previewPanel, ClearCurrentlySelectedCard);

        return card;
    }

    private FoodCard CreateFoodCard(FoodItem food)
    {
        FoodCard card = Instantiate(foodCardPrefab, recipeCardParent);
        card.SetupCard(food.Data, previewPanel, ClearCurrentlySelectedCard);
        card.Init(food, Inventory.QuantityOwned(food));

        return card;
    }

    private void ClearCurrentlySelectedCard(UICard card)
    {
        currentlySelectedCard?.ClearSelect();
        currentlySelectedCard = card;
        previewPanel.SetRecipe(card.Data as RecipeData);
        selectedIndex = spawnedCards.IndexOf(card);
    }

    private void TrySelectFirstCard()
    {
        SetFoodOnPanel(null);
        if(spawnedCards.Count > 0)
        {
            spawnedCards[0].SelectCard();
            selectedIndex = 0;
        }
    }

    private void SelectAtIndex(int index)
    {
        currentlySelectedCard.ClearSelect();
        selectedIndex = index;
        spawnedCards[index].SelectCard();
    }

    public void SetFoodOnPanel(FoodItem item)
    {
        previewPanel.SetFood(item);
    }

    // Update is called once per frame
    void Update()
    {
        MoveSelectionBasedOnInput();
    }

    private void TryShiftSelectedIndexAndSelect(int rowMovement, int columnMovement)
    {
        int rowLength = 4;
        int columns = Mathf.CeilToInt(spawnedCards.Count / 4.0f);

        int finalRowMove = rowMovement;
        int finalColumnMove = columnMovement;

        if(selectedIndex <= 4 && rowMovement < 0)
        {
            finalRowMove = 0;
        }

        if(spawnedCards.Count - selectedIndex <= rowLength)
        {
            finalRowMove = 0;
        }

        if(columnMovement < 0 && selectedIndex % 4 == 0)
        {
            finalColumnMove = 0;
        }

        if(columnMovement > 0 && (selectedIndex % 4 == 3 || selectedIndex == spawnedCards.Count - 1))
        {
            finalColumnMove = 0;
        }

        int newIndex = selectedIndex + (finalRowMove * rowLength) + (finalColumnMove);
        SelectAtIndex(newIndex);
    }


    private void MoveSelectionBasedOnInput()
    {
        if(Input.GetKeyDown(InputManager.Instance.GetAssignedCode(Inputs.StickNW)))
        {
            TryShiftSelectedIndexAndSelect(-1, -1);
        }
        else if(Input.GetKeyDown(InputManager.Instance.GetAssignedCode(Inputs.StickNN)))
        {
            TryShiftSelectedIndexAndSelect(-1, 0);
        }
        else if (Input.GetKeyDown(InputManager.Instance.GetAssignedCode(Inputs.StickNE)))
        {
            TryShiftSelectedIndexAndSelect(-1, 1);
        }
        else if (Input.GetKeyDown(InputManager.Instance.GetAssignedCode(Inputs.StickEE)))
        {
            TryShiftSelectedIndexAndSelect(0, 1);
        }
        else if (Input.GetKeyDown(InputManager.Instance.GetAssignedCode(Inputs.StickSE)))
        {
            TryShiftSelectedIndexAndSelect(1, 1);
        }
        else if (Input.GetKeyDown(InputManager.Instance.GetAssignedCode(Inputs.StickSS)))
        {
            TryShiftSelectedIndexAndSelect(1, 0);
        }
        else if (Input.GetKeyDown(InputManager.Instance.GetAssignedCode(Inputs.StickSW)))
        {
            TryShiftSelectedIndexAndSelect(1, -1);
        }
        else if (Input.GetKeyDown(InputManager.Instance.GetAssignedCode(Inputs.StickWW)))
        {
            TryShiftSelectedIndexAndSelect(0, -1);
        }
    }
}
