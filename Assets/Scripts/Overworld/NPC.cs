using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public float wordSpeed;
    public GameObject continuePanel;
    public EnemyEntityData enemy;
    // Set this file to your compiled json asset
    public TextAsset inkAsset;
    // The ink story that we're wrapping
    Story _inkStory;
    public GameObject choicesPanel;

    private string dialogue;
    private bool playerIsClose;

    void Start()
    {
        dialogueText.text = "";
    }

    void Awake()
    {
        _inkStory = new Story(inkAsset.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && playerIsClose)
        {
            if (!dialoguePanel.activeInHierarchy )
            {
                dialoguePanel.SetActive(true);
                dialogue = _inkStory.Continue();
                StartCoroutine(Typing());
            }
        }

        if (dialogueText.text == dialogue)
        {
            continuePanel.SetActive(true);

            if (Input.GetButtonDown("Interact"))
            {
                if (_inkStory.canContinue)
                {
                    NextLine();
                }
                else
                {
                    ZeroText();
                }
            }
            if (Input.GetButtonDown("Fight"))
            {
                ZeroText();
                EnterCombatWithEnemy(enemy);
            }

        }
    }

    private void EnterCombatWithEnemy(EnemyEntityData enemyData)
    {
        FightingManager.Instance.OpenFightingForEnemy(enemyData);
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        continuePanel.SetActive(false);

        if (_inkStory.canContinue)
        {
            dialogueText.text = "";
            dialogue = _inkStory.Continue();
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            ZeroText();
        }
    }
}
