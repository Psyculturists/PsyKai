using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject interactionIcon;
    public Vector3 offset;
    public TextAsset inkAsset;

    private Camera cam;
    private bool playerIsClose;

    void Start()
    {
        cam = Camera.main;
        interactionIcon.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerIsClose && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            interactionIcon.SetActive(true);
            if (Input.GetButtonDown("Interact"))
            {
                EnterDialogue();   
            }
        }
        else
        {
            interactionIcon.SetActive(false);
        }
    }

    private void EnterDialogue()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkAsset, gameObject.GetComponent<NPC>().data);
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
        }
    }
}
