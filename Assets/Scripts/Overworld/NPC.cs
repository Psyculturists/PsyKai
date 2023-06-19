using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogue;
    public float wordSpeed;
    public GameObject continuePanel;
    public EnemyEntityData enemy;

    private bool playerIsClose;
    private int index;

    void Start()
    {
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && playerIsClose)
        {
            if (!dialoguePanel.activeInHierarchy )
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        if (dialogueText.text == dialogue[index])
        {
            continuePanel.SetActive(true);

            if (Input.GetButtonDown("Interact"))
            {
                if (index < dialogue.Length - 1)
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
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        continuePanel.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
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
