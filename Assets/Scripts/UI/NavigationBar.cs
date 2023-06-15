using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationBar : MonoBehaviour
{
    public static NavigationBar Instance;

    [SerializeField]
    private GameObject HubScreen;
    [SerializeField]
    private GameObject fishingScreen;
    [SerializeField]
    private CookingUIParent cookingScreen;
    [SerializeField]
    private GameObject farmingScreen;
    [SerializeField]
    private FightingManager fightingScreen;

    [SerializeField]
    private Button fishingButton;
    [SerializeField]
    private Button cookingButton;
    [SerializeField]
    private Button farmingButton;
    [SerializeField]
    private Button fightingButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitialiseButtons();
    }

    private void InitialiseButtons()
    {
        //fishingButton.onClick.AddListener(() => OpenScreen(fishingScreen));
        cookingButton.onClick.AddListener(() => OpenCooking());
        //farmingButton.onClick.AddListener(() => OpenScreen(farmingScreen));
        fightingButton.onClick.AddListener(() => OpenFighting());
    }

    private void OpenScreen(GameObject screen)
    {
        if(screen.activeInHierarchy)
        {
            return;
        }

        //fishingScreen.SetActive(screen == fishingScreen);
        HubScreen.SetActive(screen == HubScreen);
        cookingScreen.gameObject.SetActive(screen == cookingScreen.gameObject);
        //farmingScreen.SetActive(screen == farmingScreen);
        fightingScreen.gameObject.SetActive(screen == fightingScreen.gameObject);
    }

    public void OpenHub()
    {
        OpenScreen(HubScreen.gameObject);
    }

    public void OpenCooking()
    {
        OpenScreen(cookingScreen.gameObject);
        cookingScreen.OpenForRecipes();
    }
    public void OpenFood()
    {
        OpenScreen(cookingScreen.gameObject);
        cookingScreen.OpenForFood();
    }
    public void OpenFishing()
    {
        OpenScreen(fishingScreen);
    }
    public void OpenFarming()
    {
        OpenScreen(farmingScreen);
    }
    public void OpenFighting()
    {
        OpenScreen(fightingScreen.gameObject);
        fightingScreen.OpenFightingScene();
    }

    public void OpenFoodFromBattle()
    {
        cookingScreen.gameObject.SetActive(true);
        cookingScreen.OpenForFood();
        fightingScreen.gameObject.SetActive(false);
    }
    public void ReturnFromFoodToBattle()
    {
        cookingScreen.gameObject.SetActive(false);
        fightingScreen.gameObject.SetActive(true);
    }
}
