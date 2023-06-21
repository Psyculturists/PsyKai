using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Image talkerPortrait;
    public TextMeshProUGUI name;

    [Header("Choices UI")]

    public GameObject choicesPanel;
    public GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    public GameObject continuePanel;
    public float wordSpeed;
    public bool dialogueIsPlaying { get; private set; }

    private NPCData talkerData;
    private string dialogue;
    private Story currentStory;
    private static DialogueManager instance;
    private bool canContinue;
    private InkExternalFunctions inkExternalFunctions;

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

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            choice.gameObject.SetActive(false);
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        canContinue = (dialogue == dialogueText.text);
        continuePanel.SetActive(canContinue);

        if (Input.GetButtonDown("Interact") && canContinue)
        {
            ContinueStory();
        }
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public void EnterDialogueMode(TextAsset inkJSON, NPCData entityData)
    {
        currentStory = new Story(inkJSON.text);
        talkerData = entityData;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        inkExternalFunctions.Bind(currentStory);
        talkerPortrait.sprite = talkerData.portrait;
        name.text = talkerData.name;

        ContinueStory();
    }

    public void EnterFight()
    {
        EnemyEntityData enemy = talkerData.entityData;
        if (enemy != null)
        {
            StartCoroutine(ExitDialogueMode());
            FightingManager.Instance.OpenFightingForEnemy(enemy);
        }
        else
        {
            Debug.LogWarning("A fight was triggered but no enemy data was passed.");
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

        dialogueIsPlaying = false;
        dialogueText.text = "";
        dialogue = "";
        dialoguePanel.SetActive(false);
    } 

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogue = currentStory.Continue();

            dialogueText.text = "";
            DisplayChoices();

            choicesPanel.SetActive(currentStory.currentChoices.Count > 0);

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
        if ( currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can handle. Were given " + currentChoices.Count + " choices.");
            return;
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false); 
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        currentStory.Continue();
    }
}
