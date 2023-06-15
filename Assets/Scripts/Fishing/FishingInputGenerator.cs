using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingInputGenerator : MonoBehaviour
{
    [SerializeField]
    private InputHitZone hitZone;
    [SerializeField]
    private InputElement inputElementPrefab;
    [SerializeField]
    private Transform inputElementParent;
    [SerializeField]
    private Transform elementSpawnLocation;
    [SerializeField]
    private Button startFishingButton;

    private List<InputElement> spawnedElements = new List<InputElement>();

    [SerializeField]
    private float rateOfMovement = 100.0f;

    [SerializeField]
    private Vector2 AmountOfPromptsRange = new Vector2(3, 5);

    [SerializeField]
    private DropTable fishingDropTable;
    [SerializeField]
    private ItemData requiredBait;

    private float hitRange = 60.0f;

    private bool hasValidatedThisFrame = false;

    private void Start()
    {
        startFishingButton.onClick.AddListener(BeginFishing);
    }

    private void OnDisable()
    {
        Reset();
    }

    private void BeginFishing()
    {
        if(Inventory.OwnsAtLeast(requiredBait, 1))
        {
            Inventory.UseItems(requiredBait, 1);
        }
        else
        {
            PopupManager.Instance.ShowInfoPopup("No Bait!", "You seem to have run out of bait. Go fight some foes to obtain more!");
            return;
        }

        int numberToSpawn = Random.Range((int)AmountOfPromptsRange.x, (int)AmountOfPromptsRange.y);

        for(int i = 0; i < numberToSpawn; i++)
        {
            SpawnElement();
        }
        startFishingButton.gameObject.SetActive(false);
    }

    private void SpawnElement()
    {
        // get a random input
        InputElement element = Instantiate(inputElementPrefab, inputElementParent);
        int rand = UnityEngine.Random.Range((int)Inputs.StickNW, (int)Inputs.StickWW);

        element.Initialise((Inputs)rand, ValidateInputTiming);
        spawnedElements.Add(element);

        element.transform.localPosition = elementSpawnLocation.transform.localPosition + (Vector3.right * rateOfMovement * 1.5f * (spawnedElements.Count - 1));
    }

    private void UpdateElementPositions()
    {
        foreach(InputElement element in spawnedElements)
        {
            element.transform.localPosition += new Vector3(-rateOfMovement * Time.deltaTime, 0, 0);
        }
        if(spawnedElements == null || spawnedElements.Count == 0)
        {
            return;
        }
        Vector2 firstElementPos = spawnedElements[0].transform.localPosition;
        if (Vector2.Distance(firstElementPos, hitZone.transform.localPosition) < hitRange)
        {
            hitZone.UpdateHitState(true);
        }
        else
        {
            hitZone.UpdateHitState(false);
        }
        hasValidatedThisFrame = false;
    }

    private void ValidateInputTiming(InputElement element)
    {
        if(spawnedElements[0] != element || hasValidatedThisFrame)
        {
            return;
        }
        hasValidatedThisFrame = true;
        if(Vector2.Distance(element.transform.localPosition, hitZone.transform.localPosition) < hitRange)
        {
            RemoveElement(element);
            hitZone.UpdateHitState(false);
            if(spawnedElements.Count == 0)
            {
                Reset();
                OnSuccess();
            }
            Debug.Log("passed dist check");
        }
        else
        {
            Debug.Log(spawnedElements.IndexOf(element));
            Debug.Log(spawnedElements.Count);
            Debug.Log(Vector2.Distance(element.transform.localPosition, hitZone.transform.localPosition));
            FailedFishing();
        }
    }

    private void RemoveElement(InputElement element)
    {
        spawnedElements.Remove(element);
        Destroy(element.gameObject);
    }

    private void FailedFishing()
    {
        spawnedElements.Clear();
        inputElementParent.DestroyAllChildren();
        hitZone.UpdateHitState(false);
        startFishingButton.gameObject.SetActive(true);
    }

    private void Reset()
    {
        spawnedElements.Clear();
        inputElementParent.DestroyAllChildren();
        startFishingButton.gameObject.SetActive(true);
    }

    private void OnSuccess()
    {
        ItemData reward = fishingDropTable.Evaluate();
        Inventory.GrantItem(reward, 1);
        PopupManager.Instance.ShowInfoPopup("Fish Obtained!", "You just fished up a " + reward.ItemName + "!");
    }

    private void Update()
    {
        UpdateElementPositions();
    }
}
