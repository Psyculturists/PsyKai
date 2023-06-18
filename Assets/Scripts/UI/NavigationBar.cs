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
    private EnemySelectDebug enemySelectDebug;

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
        //fightingButton.onClick.AddListener(() => OpenFighting());
        fightingButton.onClick.AddListener(() => enemySelectDebug.ToggleOpen());
    }

    private void OpenScreen(GameObject screen)
    {
        if(screen?.activeInHierarchy == true)
        {
            return;
        }

        //fishingScreen.SetActive(screen == fishingScreen);
        HubScreen.SetActive(screen == HubScreen);
        cookingScreen.gameObject.SetActive(screen == cookingScreen.gameObject);
        //farmingScreen.SetActive(screen == farmingScreen);
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

    public void OpenFightingForEnemy(EnemyEntityData data)
    {
        OpenScreen(fightingScreen.gameObject);
        fightingScreen.OpenFightingForEnemy(data);
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

    public void HideAllMenus()
    {
        OpenScreen(null);
    }

    public void LaunchMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SideScroller", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        HideAllMenus();
    }
}
