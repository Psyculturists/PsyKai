using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Image talkerPortrait;
    public TextMeshProUGUI name;

    [Header("Choices UI")]
    private GameObject[] choices;
    public GameObject choicesPanel;

    public CinemachineVirtualCamera vcCamera;
    public GameObject continuePanel;
    public float wordSpeed;
    public bool dialogueIsPlaying { get; private set; }
    public float dialogueCameraDistance;
    public TagData[] tags;

    private NPCData talkerData;
    private string dialogue;
    private Story currentStory;
    private static DialogueManager instance;
    private bool canContinue;
    private bool areChoices;
    private InkExternalFunctions inkExternalFunctions;
    private float previousCameraDistance;
    private bool isFighting;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of DialogueManager found!");
        }
        instance = this;

        inkExternalFunctions = new InkExternalFunctions();
    }

    private void Start()
    {
        dialogue = "";
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        continuePanel.SetActive(false);
        canContinue = false;
        areChoices = false;

        choices = new GameObject[choicesPanel.transform.childCount];
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i] = choicesPanel.transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        canContinue = (dialogue == dialogueText.text);
        areChoices = (currentStory.currentChoices.Count > 0 && canContinue);

        continuePanel.SetActive(canContinue);
        choicesPanel.SetActive(areChoices);
        bool isFighting = FightingManager.Instance.isFighting;

        if (canContinue && !areChoices && !isFighting && (Input.GetButtonDown("Interact") || Input.GetMouseButtonDown(0)))
        {
            ContinueStory();
        }
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        inkExternalFunctions.Bind(currentStory);

        StartCoroutine(CenterCamera());
        previousCameraDistance = vcCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
        vcCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = dialogueCameraDistance;

        ContinueStory();
    }

    private void ChangeTalker(string tag)
    {
        foreach (TagData item in tags)
        {
            if (tag == item.tag)
            {
                talkerPortrait.sprite = item.characterData.portrait;
                name.text = item.characterData.name;
            }
        }
    }

    IEnumerator CenterCamera()
    {
        var composer = vcCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        float deadZoneWidth = composer.m_DeadZoneWidth;
        float deadZoneHeight = composer.m_DeadZoneHeight;

        composer.m_DeadZoneWidth = 0f;
        composer.m_DeadZoneHeight = 0f;

        yield return new WaitForSeconds(1.25f);

        composer.m_DeadZoneWidth = deadZoneWidth;
        composer.m_DeadZoneHeight = deadZoneHeight;
    }

    public void EnterFight(string tag)
    {
        EnemyEntityData enemy = new EnemyEntityData();
        foreach (TagData item in tags)
        {
            if (tag == item.tag)
            {
                enemy = item.characterData.entityData;
            }
        }
        if (enemy != null)
        {
            Debug.Log("Fight!");
            FightingManager.Instance.OpenFightingForEnemy(enemy);
        }
        else
        {
            Debug.LogWarning("A fight was triggered but I couldnt find that character.");
        }
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.1f);

        inkExternalFunctions.Unbind(currentStory);

        vcCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = previousCameraDistance;

        dialogueIsPlaying = false;
        dialogueText.text = "";
        dialogue = "";
        dialoguePanel.SetActive(false);
    } 

    private void ContinueStory()
    {
        Debug.Log("Continuing story...");
        if (currentStory.canContinue)
        {
            dialogue = currentStory.Continue();

            if (currentStory.currentTags.Count > 0)
            {
                ChangeTalker(currentStory.currentTags[0]);
            }

            dialogueText.text = "";
            if (currentStory.currentChoices.Count > 0)
            {
                DisplayChoices();
            }

            if (dialogue.Equals("") && !currentStory.canContinue)
            {
                StartCoroutine(ExitDialogueMode());
            }
            else
            {
                StartCoroutine(Typing());
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("You sent me more choices than I can handle. What are you doing? Tell Kraith about this.");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choices[index].GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice(choices[0]));
    }

    private IEnumerator SelectFirstChoice(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
